using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;

namespace Chameleon.Domain
{
    public interface IMetaFieldService : IMetaObjectCommonServiceBase<MetaField>
    {
        /// <summary>
        /// 预置系统字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="metaObjectCode"></param>
        /// <param name="applicationId"></param>
        void PresetSystemFields(Guid metaObjectId, string metaObjectCode, Guid applicationId);
        /// <summary>
        /// 校验字段和值是否类型一致
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Result<dynamic> CheckAndGetFieldValueByFieldType(Guid fieldId, object value);
        /// <summary>
        /// 校验字段和值是否类型一致
        /// </summary>
        /// <param name="metaField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Result<dynamic> CheckAndGetFieldValueByFieldType(MetaField metaField, object value);
    }

    public class MetaFieldService : MetaObjectCommonServiceBase<MetaField>, IMetaFieldService
    {
        public MetaFieldService(IMetaFieldRepository repository) : base(repository)
        {
        }

        public void PresetSystemFields(Guid metaObjectId, string metaObjectCode, Guid applicationId)
        {
            Ensure.ArgumentNotNullOrEmpty(metaObjectId, nameof(metaObjectId));
            Ensure.ArgumentNotNullOrEmpty(metaObjectCode, nameof(metaObjectCode));
            Ensure.ArgumentNotNullOrEmpty(applicationId, nameof(applicationId));

            var systemFields = new List<MetaField> {
                new MetaField{
                    ShortCode ="_id",
                    Name ="数据ID",
                    FieldType=(int)DataType.Text,
                },
                new MetaField{
                    ShortCode ="IsDeleted",
                    Name ="是否删除",
                    FieldType=(int)DataType.Boolean,
                },
                new MetaField{
                    ShortCode ="CreateBy",
                    Name ="创建人",
                    FieldType= (int)DataType.Int32,
                },
                new MetaField{
                    ShortCode ="CreateTime",
                    Name ="创建时间",
                    FieldType= (int)DataType.DateTime,
                },
                new MetaField{
                    ShortCode ="ModifyBy",
                    Name ="修改人",
                    FieldType= (int)DataType.Int32,
                },
                new MetaField{
                    ShortCode ="ModifyTime",
                    Name ="修改时间",
                    FieldType= (int)DataType.DateTime,
                }
            };

            systemFields.ForEach(item =>
            {
                item.Id = Guid.NewGuid();
                item.CloudApplicationtId = applicationId;
                item.MetaObjectId = metaObjectId;
                item.Code = string.Concat(metaObjectCode, ".", item.ShortCode);
                item.IsSystem = 1;
                item.Group = "系统";
                item.CreateBy = item.ModifyBy = 0;
                item.CreateTime = item.ModifyTime = DateTime.Now;
                item.SortNumber = -1;
                item.Description = "系统字段";
            });

            base._commonRepositoryBase.BatchAdd(systemFields);
        }

        /// <summary>
        /// 同方法内多次调用该方法不要直接用这个查询数据库，性能较差，应该通过对象查出所有对象用下面的重载方法
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Result<dynamic> CheckAndGetFieldValueByFieldType(Guid fieldId, object value)
        {
            MetaField metaField = _commonRepositoryBase.GetById(fieldId);
            return CheckAndGetFieldValueByFieldType(metaField, value);
        }

        public Result<dynamic> CheckAndGetFieldValueByFieldType(MetaField metaField, object value)
        {
            dynamic resultData = null;
            bool isSuccess = false;
            switch ((DataType)metaField.FieldType)
            {
                case DataType.Boolean:
                    isSuccess = bool.TryParse(Convert.ToString(value), out bool boolVal);
                    resultData = boolVal;
                    break;
                case DataType.Int32:
                    isSuccess = int.TryParse(Convert.ToString(value), out int number);
                    if (number < 0)
                        isSuccess = false;
                    resultData = number;
                    break;
                case DataType.DateTime:
                case DataType.Date:
                    isSuccess = DateTime.TryParse(Convert.ToString(value), out DateTime dateTimeVal);
                    resultData = dateTimeVal;
                    break;
                case DataType.Int64:
                    isSuccess = long.TryParse(Convert.ToString(value), out long longVal);
                    resultData = longVal;
                    break;
                case DataType.Double:
                    isSuccess = double.TryParse(Convert.ToString(value), out double doubleVal);
                    resultData = doubleVal;
                    break;
                case DataType.DataSource:
                    isSuccess = false;
                    break;
                case DataType.Decimal:
                    isSuccess = decimal.TryParse(Convert.ToString(value), out decimal decimalVal);
                    resultData = decimalVal;
                    break;
                case DataType.Unknown:
                    isSuccess = false;
                    break;
                case DataType.Text:
                default:
                    isSuccess = true;
                    resultData = Convert.ToString(value);
                    break;
            }

            return isSuccess ? Result<dynamic>.Success(data: resultData) : Result<dynamic>.Error(data: resultData);
        }
    }
}
