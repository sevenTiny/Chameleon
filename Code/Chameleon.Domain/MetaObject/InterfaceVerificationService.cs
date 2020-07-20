using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using System;
using System.Text.RegularExpressions;

namespace Chameleon.Domain
{
    public interface IInterfaceVerificationService : IMetaObjectCommonServiceBase<InterfaceVerification>
    {
        /// <summary>
        /// 添加校验项
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result AddVerificationItem(InterfaceVerification entity);
        /// <summary>
        /// 更新校验项
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result UpdateVerificationItem(InterfaceVerification entity);
        /// <summary>
        /// 正则表达式校验输入值是否正确
        /// </summary>
        /// <param name="interfaceVerification"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        bool IsMatch(InterfaceVerification interfaceVerification, string input);
    }

    public class InterfaceVerificationService : MetaObjectCommonServiceBase<InterfaceVerification>, IInterfaceVerificationService
    {
        IInterfaceVerificationRepository _InterfaceVerificationRepository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceVerificationService(IInterfaceVerificationRepository repository, IMetaFieldRepository metaFieldRepository) : base(repository)
        {
            _metaFieldRepository = metaFieldRepository;
            _InterfaceVerificationRepository = repository;
        }

        public Result AddVerificationItem(InterfaceVerification entity)
        {
            //查询字段
            var fieldDic = _metaFieldRepository.GetMetaFieldIdDicByMetaObjectId(entity.MetaObjectId);

            if (!fieldDic.ContainsKey(entity.MetaFieldId))
                return Result.Error("当前对象中不存在该字段");

            var metaField = fieldDic[entity.MetaFieldId];

            entity.Name = metaField.Name;
            //短编码重新赋值
            entity.MetaFieldShortCode = metaField.ShortCode;

            //校验是否已经存在了这个字段的校验
            if (_InterfaceVerificationRepository.CheckMetaFieldShortCodeHasExistInCurrentVerification(entity.ParentId, entity.MetaFieldShortCode))
                return Result.Error("已经存在一个该字段的规则了");

            return base.AddNoCareCode(entity);
        }

        public Result UpdateVerificationItem(InterfaceVerification entity)
        {
            return base.UpdateWithId(entity.Id, item =>
            {
                item.VerificationTips = entity.VerificationTips;
                item.RegularExpression = entity.RegularExpression;
                item.RegularType = entity.RegularType;
            });
        }

        public bool IsMatch(InterfaceVerification interfaceVerification, string input)
        {
            switch (interfaceVerification.GetRegularType())
            {
                //如果是自定义的，则走自定义校验
                case RegularTypeEnum.Custom when !string.IsNullOrEmpty(interfaceVerification.RegularExpression):
                    return input.IsXXX_ByRegex(interfaceVerification.RegularExpression);
                case RegularTypeEnum.NotNullOrEmpty:
                    return !string.IsNullOrEmpty(input);
                case RegularTypeEnum.Email:
                    return input.IsEmail();
                case RegularTypeEnum.TelPhone:
                    return input.IsTelPhone();
                case RegularTypeEnum.MobilePhone:
                    return input.IsMobilePhone();
                case RegularTypeEnum.URL:
                    return input.IsURL();
                case RegularTypeEnum.IpAddress:
                    return input.IsIpAddress();
                case RegularTypeEnum.ID_Card:
                    return input.IsID_Card();
                case RegularTypeEnum.AccountName:
                    return input.IsAccountName();
                case RegularTypeEnum.Password:
                    return input.IsPassword();
                case RegularTypeEnum.StrongCipher:
                    return input.IsStrongCipher();
                case RegularTypeEnum.DataFormat:
                    return input.IsDataFormat();
                case RegularTypeEnum.ChineseCharactor:
                    return input.IsChineseCharactor();
                case RegularTypeEnum.QQ_Number:
                    return input.IsQQ_Number();
                case RegularTypeEnum.PostalCode:
                    return input.IsPostalCode();
                default:
                    throw new ArgumentException("regular type not found");
            }
        }
    }
}
