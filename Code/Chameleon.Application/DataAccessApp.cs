using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Chameleon.ValueObject;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.Application
{
    public interface IDataAccessApp
    {
        /// <summary>
        /// 翻译BsonDocument为CloudData模式
        /// </summary>
        /// <param name="bsonDocuments"></param>
        /// <param name="formatInterfaceFieldsDic">如有必要使用接口返回设置过滤返回值数量，则传递该参数</param>
        /// <returns></returns>
        List<Dictionary<string, CloudData>> TranslatorBsonToCloudData(List<BsonDocument> bsonDocuments, Dictionary<string, InterfaceFields> formatInterfaceFieldsDic = null);
        /// <summary>
        /// 翻译BsonDocument为CloudData模式
        /// </summary>
        /// <param name="bsonDocument"></param>
        /// <param name="formatInterfaceFieldsDic">如有必要使用接口返回设置过滤返回值数量，则传递该参数</param>
        /// <returns></returns>
        Dictionary<string, CloudData> TranslatorBsonToCloudData(BsonDocument bsonDocument, Dictionary<string, InterfaceFields> formatInterfaceFieldsDic = null);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="interfaceSetting"></param>
        /// <param name="bsonsList"></param>
        /// <returns></returns>
        Result BatchAdd(InterfaceSetting interfaceSetting, IEnumerable<BsonDocument> bsonsList);
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="interfaceSetting"></param>
        /// <param name="condition"></param>
        /// <param name="bsonDocument"></param>
        /// <returns></returns>
        Result BatchUpdate(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter, BsonDocument bsonDocument);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="interfaceSetting"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Result Delete(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter);
        /// <summary>
        /// 获取单条，内部走的获取列表
        /// </summary>
        /// <param name="interfaceSetting"></param>
        /// <param name="condition"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        Result<Dictionary<string, CloudData>> Get(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="interfaceSetting"></param>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortSetting"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        Result<List<Dictionary<string, CloudData>>> GetList(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter, int pageIndex = 0);
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="interfaceSetting"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Result<int> GetCount(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter);
    }

    public class DataAccessApp : IDataAccessApp
    {
        IInterfaceConditionService _interfaceConditionService;
        IInterfaceFieldsService _interfaceFieldsService;
        IMetaFieldService _metaFieldService;
        IInterfaceSettingService _interfaceSettingService;
        ChameleonDataDbContext _chameleonDataDbContext;
        IMetaFieldRepository _metaFieldRepository;
        IInterfaceVerificationRepository _interfaceVerificationRepository;
        IInterfaceVerificationService _interfaceVerificationService;
        IInterfaceFieldsRepository _interfaceFieldsRepository;
        public DataAccessApp(IInterfaceFieldsRepository interfaceFieldsRepository, IInterfaceVerificationRepository interfaceVerificationRepository, IInterfaceVerificationService interfaceVerificationService, IMetaFieldRepository metaFieldRepository, ChameleonDataDbContext chameleonDataDbContext, IInterfaceSettingService interfaceSettingService, IInterfaceConditionService interfaceConditionService, IInterfaceFieldsService interfaceFieldsService, IMetaFieldService metaFieldService)
        {
            _interfaceFieldsRepository = interfaceFieldsRepository;
            _interfaceVerificationRepository = interfaceVerificationRepository;
            _interfaceVerificationService = interfaceVerificationService;
            _metaFieldRepository = metaFieldRepository;
            _chameleonDataDbContext = chameleonDataDbContext;
            _interfaceSettingService = interfaceSettingService;
            _metaFieldService = metaFieldService;
            _interfaceFieldsService = interfaceFieldsService;
            _interfaceConditionService = interfaceConditionService;
        }

        /// <summary>
        /// 构造排序
        /// </summary>
        /// <param name="metaFieldKeyUpperDic"></param>
        /// <param name="sortSetting">key为字段，value为DESC或ASC</param>
        /// <returns></returns>
        private SortDefinition<BsonDocument> StructureSortDefinition(Dictionary<string, MetaField> metaFieldKeyUpperDic, Dictionary<string, string> sortSetting)
        {
            var builder = new SortDefinitionBuilder<BsonDocument>();

            if (sortSetting == null || !sortSetting.Any())
                return builder.Ascending("_id");

            SortDefinition<BsonDocument> sort = null;

            foreach (var item in sortSetting)
            {
                if (!metaFieldKeyUpperDic.ContainsKey(item.Key.ToUpperInvariant()))
                    throw new KeyNotFoundException($"field {item.Key} is not exist in current MetaObject MetaField List");

                //DESC
                if ("DESC".Equals(item.Value.ToUpperInvariant()))
                {
                    if (sort == null)
                        sort = builder.Descending(item.Key);
                    else
                        sort = sort.Descending(item.Key);
                }
                //ASC
                else
                {
                    if (sort == null)
                        sort = builder.Ascending(item.Key);
                    else
                        sort = sort.Ascending(item.Key);
                }
            }

            return sort;
        }

        public List<Dictionary<string, CloudData>> TranslatorBsonToCloudData(List<BsonDocument> bsonDocuments, Dictionary<string, InterfaceFields> formatInterfaceFieldsDic = null)
        {
            if (bsonDocuments == null || !bsonDocuments.Any())
                return new List<Dictionary<string, CloudData>>(0);

            return bsonDocuments.Select(t => TranslatorBsonToCloudData(t, formatInterfaceFieldsDic)).ToList();
        }

        public Dictionary<string, CloudData> TranslatorBsonToCloudData(BsonDocument bsonDocument, Dictionary<string, InterfaceFields> formatInterfaceFieldsDic = null)
        {
            if (bsonDocument == null || !bsonDocument.Any())
                return new Dictionary<string, CloudData>(0);

            Dictionary<string, CloudData> result = new Dictionary<string, CloudData>(formatInterfaceFieldsDic?.Count ?? bsonDocument.ElementCount);

            foreach (var element in bsonDocument)
            {
                CloudData cloudData = null;

                //如果有自定义返回设置
                if (formatInterfaceFieldsDic != null)
                {
                    var upperKey = element.Name.ToUpperInvariant();

                    //如果自定义返回设置里没有该字段，则不返回
                    if (!formatInterfaceFieldsDic.ContainsKey(upperKey))
                        continue;

                    cloudData = new CloudData
                    {
                        FieldText = formatInterfaceFieldsDic[upperKey].MetaFieldCustomViewName
                    };
                }

                if (cloudData == null)
                    cloudData = new CloudData();

                cloudData.FieldCode = element.Name;
                cloudData.Value = element.Value?.ToString();
                //如果后续需要翻译，则处理该字段
                cloudData.ValueText = cloudData.Value;

                result.TryAdd(element.Name, cloudData);
            }

            return result;
        }

        public Result BatchAdd(InterfaceSetting interfaceSetting, IEnumerable<BsonDocument> bsonsList)
        {
            if (interfaceSetting == null)
                return Result.Error("接口设置参数不能为空");

            if (bsonsList == null || !bsonsList.Any())
                return Result.Success($"没有任何数据需要插入");

            //获取全部接口校验
            var verificationDic = _interfaceVerificationRepository.GetMetaFieldUpperKeyDicByInterfaceVerificationId(interfaceSetting.InterfaceVerificationId);

            //获取到字段列表以编码为Key大写的字典
            var metaFields = _metaFieldRepository.GetMetaFieldShortCodeUpperDicByMetaObjectId(interfaceSetting.MetaObjectId);

            List<BsonDocument> insertBsonsList = new List<BsonDocument>(bsonsList.Count());

            foreach (var bsonDocument in bsonsList)
            {
                if (bsonDocument == null)
                    return Result.Error("数据为空，插入终止");

                BsonDocument bsonElementsToAdd = new BsonDocument();

                foreach (var bsonElement in bsonDocument)
                {
                    string upperKey = bsonElement.Name.ToUpperInvariant();

                    if (!metaFields.ContainsKey(upperKey))
                        continue;

                    //校验值是否符合接口校验设置
                    if (verificationDic.ContainsKey(upperKey))
                    {
                        var verification = verificationDic[upperKey];
                        if (!_interfaceVerificationService.IsMatch(verification, Convert.ToString(bsonElement.Value)))
                            return Result.Error(!string.IsNullOrEmpty(verification.VerificationTips) ? verification.VerificationTips : $"字段[{bsonElement.Name}]传递的值[{bsonElement.Value}]格式不正确");
                    }

                    //检查字段的值是否符合字段类型
                    var checkResult = _metaFieldService.CheckAndGetFieldValueByFieldType(metaFields[upperKey], bsonElement.Value);

                    if (checkResult.IsSuccess)
                        bsonElementsToAdd.Add(new BsonElement(metaFields[upperKey].ShortCode, BsonValue.Create(checkResult.Data)));
                    else
                        return Result.Error($"字段[{bsonElement.Name}]传递的值[{bsonElement.Value}]不符合字段定义的类型");
                }

                //获取系统内置的bson元素
                var systemBsonDocument = _metaFieldService.GetSystemFieldBsonDocument();

                //设置系统字段及其默认值
                foreach (var presetBsonElement in systemBsonDocument)
                {
                    //如果传入的字段已经有了，那么这里就不预置了
                    if (!bsonElementsToAdd.Contains(presetBsonElement.Name))
                        bsonElementsToAdd.Add(presetBsonElement);
                }

                //补充字段
                bsonElementsToAdd.SetElement(new BsonElement("MetaObject", interfaceSetting.MetaObjectCode));

                if (bsonElementsToAdd.Any())
                    insertBsonsList.Add(bsonElementsToAdd);
            }

            if (insertBsonsList.Any())
                _chameleonDataDbContext.GetCollectionBson(interfaceSetting.MetaObjectCode).InsertMany(insertBsonsList);

            return Result.Success($"插入成功! 成功{insertBsonsList.Count}条，失败{bsonsList.Count() - insertBsonsList.Count}条.");
        }

        public Result BatchUpdate(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter, BsonDocument bsonDocument)
        {
            if (interfaceSetting == null)
                return Result.Error("接口设置参数不能为空");

            if (bsonDocument == null)
                return Result.Error("数据为空，更新终止");

            //获取全部接口校验
            var verificationDic = _interfaceVerificationRepository.GetMetaFieldUpperKeyDicByInterfaceVerificationId(interfaceSetting.InterfaceVerificationId);

            //获取到字段列表以编码为Key大写的字典
            var metaFields = _metaFieldRepository.GetMetaFieldShortCodeUpperDicByMetaObjectId(interfaceSetting.MetaObjectId);

            BsonDocument bsonElementsToModify = new BsonDocument();

            foreach (var bsonElement in bsonDocument)
            {
                string upperKey = bsonElement.Name.ToUpperInvariant();

                if (!metaFields.ContainsKey(upperKey))
                    continue;

                //校验值是否符合接口校验设置
                if (verificationDic.ContainsKey(upperKey))
                {
                    var verification = verificationDic[upperKey];

                    if (!_interfaceVerificationService.IsMatch(verification, Convert.ToString(bsonElement.Value)))
                        return Result.Error(!string.IsNullOrEmpty(verification.VerificationTips) ? verification.VerificationTips : $"字段[{bsonElement.Name}]传递的值[{bsonElement.Value}]格式不正确");
                }

                //检查字段的值是否符合字段类型
                var checkResult = _metaFieldService.CheckAndGetFieldValueByFieldType(metaFields[upperKey], bsonElement.Value);

                if (checkResult.IsSuccess)
                    bsonElementsToModify.Add(new BsonElement(metaFields[upperKey].ShortCode, BsonValue.Create(checkResult.Data)));
                else
                    return Result.Error($"字段[{bsonElement.Name}]传递的值[{bsonElement.Value}]不符合字段定义的类型");
            }

            //设置更新并执行更新操作
            var updateDefinitions = bsonElementsToModify.Select(item => Builders<BsonDocument>.Update.Set(item.Name, item.Value)).ToArray();

            _chameleonDataDbContext.GetCollectionBson(interfaceSetting.MetaObjectCode).UpdateMany(filter, Builders<BsonDocument>.Update.Combine(updateDefinitions));

            return Result.Success($"修改成功");
        }

        public Result Delete(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter)
        {
            _chameleonDataDbContext.GetCollectionBson(interfaceSetting.MetaObjectCode).DeleteMany(filter);

            return Result.Success($"删除成功");
        }

        public Result<Dictionary<string, CloudData>> Get(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter)
        {
            var listResult = GetList(interfaceSetting, filter, 0);

            if (listResult.IsSuccess)
                return Result<Dictionary<string, CloudData>>.Success("查询成功", listResult.Data.FirstOrDefault());

            return Result<Dictionary<string, CloudData>>.Error(listResult.Message);
        }

        public Result<List<Dictionary<string, CloudData>>> GetList(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter, int pageIndex = 0)
        {
            //获取到字段列表以编码为Key大写的字典
            var metaFields = _metaFieldRepository.GetMetaFieldShortCodeUpperDicByMetaObjectId(interfaceSetting.MetaObjectId);

            //自定义查询列表
            var interfaceFields = _interfaceFieldsRepository.GetInterfaceFieldMetaFieldUpperKeyDicByInterfaceFieldsId(interfaceSetting.InterfaceFieldsId);

            //处理查询列
            var projection = Builders<BsonDocument>.Projection.Include("_id");

            foreach (var item in interfaceFields.Values)
                projection = projection.Include(item.MetaFieldShortCode);

            //处理排序（这里后续从配置获取）
            Dictionary<string, string> sortSetting = null;

            int skipSize = (pageIndex - 1) > 0 ? ((pageIndex - 1) * interfaceSetting.PageSize) : 0;

            var datas = TranslatorBsonToCloudData(_chameleonDataDbContext.GetCollectionBson(interfaceSetting.MetaObjectCode).Find(filter).Skip(skipSize).Limit(interfaceSetting.PageSize).Sort(StructureSortDefinition(metaFields, sortSetting)).Project(projection).ToList() ?? new List<BsonDocument>(0), interfaceFields);

            var result = Result<List<Dictionary<string, CloudData>>>.Success($"查询成功，共{datas.Count}条记录", datas);

            result.MoreMessage = datas.Count.ToString();

            return result;
        }

        public Result<int> GetCount(InterfaceSetting interfaceSetting, FilterDefinition<BsonDocument> filter)
        {
            return Result<int>.Success("查询成功", Convert.ToInt32(_chameleonDataDbContext.GetCollectionBson(interfaceSetting.MetaObjectCode).CountDocuments(filter)));
        }
    }
}
