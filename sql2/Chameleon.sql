/*
Navicat MySQL Data Transfer

Source Server         : tecant_81.68.136.47
Source Server Version : 80020
Source Host           : 81.68.136.47:39901
Source Database       : Chameleon

Target Server Type    : MYSQL
Target Server Version : 80020
File Encoding         : 65001

Date: 2020-07-01 18:35:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for CloudApplication
-- ----------------------------
DROP TABLE IF EXISTS `CloudApplication`;
CREATE TABLE `CloudApplication` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `Icon` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of CloudApplication
-- ----------------------------
INSERT INTO `CloudApplication` VALUES ('101e0838-c146-49df-ac22-53c1539ba086', 'SevenTinyTest2', '7tiny开发调试2', null, null, '0', '0', '0', '2020-06-06 17:19:32', '0', '2020-06-06 17:19:32', '0', 'cloud');
INSERT INTO `CloudApplication` VALUES ('a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '7tiny开发调试', 'SevenTinyTest', '7tiny应用', '0', '0', '0', '2020-06-06 17:16:01', '0', '2020-06-29 07:15:19', '0', 'cloud');

-- ----------------------------
-- Table structure for CommonBaseToCopy
-- ----------------------------
DROP TABLE IF EXISTS `CommonBaseToCopy`;
CREATE TABLE `CommonBaseToCopy` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of CommonBaseToCopy
-- ----------------------------

-- ----------------------------
-- Table structure for InterfaceCondition
-- ----------------------------
DROP TABLE IF EXISTS `InterfaceCondition`;
CREATE TABLE `InterfaceCondition` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `MetaObjectId` char(36) NOT NULL,
  `CloudApplicationtId` char(36) NOT NULL,
  `BelongToCondition` char(36) NOT NULL,
  `ParentId` char(36) NOT NULL,
  `ConditionNodeType` int DEFAULT NULL,
  `MetaFieldId` char(36) NOT NULL,
  `MetaFieldShortCode` varchar(255) DEFAULT NULL,
  `ConditionType` int NOT NULL,
  `ConditionJointType` int NOT NULL,
  `ConditionValueType` int NOT NULL,
  `ConditionValue` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of InterfaceCondition
-- ----------------------------
INSERT INTO `InterfaceCondition` VALUES ('03b1fd62-c96b-4316-92ec-11a47a0222cf', '06caca2c-e066-40b5-94ab-a9100ec8bfbb', 'Age > ?', '', '', '0', '0', '0', '2020-06-29 05:54:09', '0', '2020-06-29 05:54:09', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'a1a69a53-7bc2-406f-acf9-207f387ccf5b', '00000000-0000-0000-0000-000000000000', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '3', '0', '1', '?');
INSERT INTO `InterfaceCondition` VALUES ('0b05caf1-268d-4315-b554-90ae8f7d9e0f', '4bac9b5b-90aa-4afe-8186-70b3cb3a2567', '&&', '', '', '0', '0', '0', '2020-06-26 13:26:25', '0', '2020-06-26 13:26:25', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'bf3390a3-b11e-477c-9e08-d01bbcdef9ac', '00000000-0000-0000-0000-000000000000', '1', '00000000-0000-0000-0000-000000000000', '-1', '5', '1', '-1', '-1');
INSERT INTO `InterfaceCondition` VALUES ('1ec282ae-10ae-4977-a587-ed308f27105e', '427e5db5-d2de-4ddf-a985-e502655b633a', 'IsDeleted == false', '', '', '0', '0', '0', '2020-06-29 04:39:39', '0', '2020-06-29 04:43:08', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '86b117d0-a668-4fa5-83ee-ce560de2a7eb', '466d4421-1c0d-4f43-898b-b3d4e883cbfb', '2', 'd5f9b763-ddd2-4ea2-bfb4-eaecb98ce41e', 'IsDeleted', '1', '0', '2', 'false');
INSERT INTO `InterfaceCondition` VALUES ('2bf0d540-eb2f-4665-8ed9-82de627d3945', 'b65e8921-3aa4-4d81-af6a-1b26c8b3b4be', 'Age > ?', '', '', '0', '0', '0', '2020-06-29 04:43:08', '0', '2020-06-29 04:43:08', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '86b117d0-a668-4fa5-83ee-ce560de2a7eb', '466d4421-1c0d-4f43-898b-b3d4e883cbfb', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '3', '0', '1', '?');
INSERT INTO `InterfaceCondition` VALUES ('385e8721-4634-44e5-8870-bb485fcd9624', 'SevenTinyTest.UserInformation.UnDeletedAndAgeRatherThanFromArg', '未删除且年龄大于参数', null, null, '0', '0', '0', '2020-06-22 21:33:14', '0', '2020-06-22 21:33:14', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('466d4421-1c0d-4f43-898b-b3d4e883cbfb', '3e535681-2286-450e-b604-a49bcfb2f56c', '&&', '', '', '0', '0', '0', '2020-06-29 04:43:08', '0', '2020-06-29 04:43:08', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '86b117d0-a668-4fa5-83ee-ce560de2a7eb', '00000000-0000-0000-0000-000000000000', '1', '00000000-0000-0000-0000-000000000000', '-1', '3', '1', '-1', '-1');
INSERT INTO `InterfaceCondition` VALUES ('48b41bb2-5dae-4e54-8e37-35b0e90f7096', '8aec13f9-e3f9-4da6-9339-4e3ae6b639b5', '&&', '', '', '0', '0', '0', '2020-06-22 21:33:38', '0', '2020-06-22 21:33:38', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '385e8721-4634-44e5-8870-bb485fcd9624', '00000000-0000-0000-0000-000000000000', '1', '00000000-0000-0000-0000-000000000000', '-1', '5', '1', '-1', '-1');
INSERT INTO `InterfaceCondition` VALUES ('54584fc6-a354-489d-9df1-18e1be28b44b', 'ec522cc9-d2dd-492a-bf79-1da5f8ddeb2a', 'IsDeleted == false', '', '', '0', '0', '0', '2020-06-26 13:24:32', '0', '2020-06-26 13:26:25', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'bf3390a3-b11e-477c-9e08-d01bbcdef9ac', '0b05caf1-268d-4315-b554-90ae8f7d9e0f', '2', 'd5f9b763-ddd2-4ea2-bfb4-eaecb98ce41e', 'IsDeleted', '1', '0', '2', 'false');
INSERT INTO `InterfaceCondition` VALUES ('5b2da092-cd7a-4f9a-a1ad-0fdbf6c03455', '74c4a9a4-4e83-4282-9667-cd4d77eee329', 'Age >= ?', '', '', '0', '0', '0', '2020-06-22 21:33:38', '0', '2020-06-22 21:33:38', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '385e8721-4634-44e5-8870-bb485fcd9624', '48b41bb2-5dae-4e54-8e37-35b0e90f7096', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '5', '0', '1', '?');
INSERT INTO `InterfaceCondition` VALUES ('66dff234-3c3a-4639-801b-572ce7c83143', 'aee7dcdd-98b7-4ee3-993a-12a8895278eb', 'Age == ?', '', '', '0', '0', '0', '2020-06-22 22:12:44', '0', '2020-06-22 22:12:44', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'a4284376-6331-47ca-be6c-b51f89869a42', '00000000-0000-0000-0000-000000000000', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '1', '0', '1', '?');
INSERT INTO `InterfaceCondition` VALUES ('86b117d0-a668-4fa5-83ee-ce560de2a7eb', 'SevenTinyTest.UserInformation.AgeRatherThan95', '未删除年龄>95', null, null, '0', '0', '0', '2020-06-29 04:39:23', '0', '2020-06-29 04:39:23', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('9495d126-2c6c-4776-89dc-ad45183918fc', 'd95e889b-5bf8-496c-84bd-95167525eb73', 'StartTime > 2020 01 01', '', '', '0', '0', '0', '2020-06-07 22:38:55', '0', '2020-06-07 22:39:13', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'e1356bf9-4fac-402c-8503-8453ba9e553e', 'f3fe067e-a9ef-4bc8-9fb0-d0dff046df0b', '2', '02c57ac8-3b04-40e4-9e28-470f62aef858', 'StartTime', '3', '0', '2', '2020 01 01');
INSERT INTO `InterfaceCondition` VALUES ('9a30e735-5c67-4dde-bbc2-4cfd310e199c', '5f02a1f3-b3ca-40be-ae27-85b6baaadcb2', '&&', '', '', '0', '0', '0', '2020-06-07 22:29:20', '0', '2020-06-29 05:02:10', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'b02079e2-5d5e-46a0-a549-a17e25e81389', 'ca3606db-88a1-43bf-8bc0-d024db43b15f', '1', '00000000-0000-0000-0000-000000000000', '-1', '1', '1', '-1', '-1');
INSERT INTO `InterfaceCondition` VALUES ('a1a69a53-7bc2-406f-acf9-207f387ccf5b', 'SevenTinyTest.UserInformation.Info', '员工信息', null, null, '0', '0', '0', '2020-06-29 05:49:04', '0', '2020-06-29 05:49:04', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('a4284376-6331-47ca-be6c-b51f89869a42', 'SevenTinyTest.UserInformation.AgeEqaul', '年龄=', null, null, '0', '0', '0', '2020-06-22 22:12:14', '0', '2020-06-22 22:12:14', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('b02079e2-5d5e-46a0-a549-a17e25e81389', 'SevenTinyTest.UserInformation.UnDeleted', '未删除且年龄大于10', '1112', null, '0', '0', '0', '2020-06-07 11:05:04', '0', '2020-06-22 20:50:28', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('b4aa9122-43cd-470d-8d6f-75285522935b', '9040cef6-eeca-44b2-b8f0-47b0d5fe6d54', 'EndTime <= 2020 12 31', '', '', '0', '0', '0', '2020-06-07 22:39:13', '0', '2020-06-07 22:39:13', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'e1356bf9-4fac-402c-8503-8453ba9e553e', 'f3fe067e-a9ef-4bc8-9fb0-d0dff046df0b', '2', 'a2cbb891-7573-4ca8-95d5-f9f932bdd39f', 'EndTime', '6', '0', '2', '2020 12 31');
INSERT INTO `InterfaceCondition` VALUES ('b7a0e47f-bf62-4cd0-a938-b812f0223748', '9e4751ee-0f76-49cc-ae41-50eff2b8e347', 'Age < 15', '', '', '0', '0', '0', '2020-06-29 05:01:43', '0', '2020-06-29 05:01:43', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'b02079e2-5d5e-46a0-a549-a17e25e81389', '9a30e735-5c67-4dde-bbc2-4cfd310e199c', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '4', '0', '2', '15');
INSERT INTO `InterfaceCondition` VALUES ('bf3390a3-b11e-477c-9e08-d01bbcdef9ac', 'SevenTinyTest.UserInformation.AgeThan90', '年龄大于90', null, null, '0', '0', '0', '2020-06-26 13:24:17', '0', '2020-06-26 13:24:17', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('c2923718-35bb-4afe-b55e-98dcf593bec2', 'SevenTinyTest.UserInformation.InfoVerify', '员工信息校验', null, null, '0', '1', '0', '2020-06-29 05:37:52', '0', '2020-06-29 05:37:52', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('c97a5828-4fd4-49f1-a6e0-454977b0d2b1', 'b4d9bf4b-78b9-4fc0-a5f3-02221ff2c8e3', 'IsDeleted == false', '', '', '0', '0', '0', '2020-06-22 21:33:28', '0', '2020-06-22 21:33:38', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '385e8721-4634-44e5-8870-bb485fcd9624', '48b41bb2-5dae-4e54-8e37-35b0e90f7096', '2', 'd5f9b763-ddd2-4ea2-bfb4-eaecb98ce41e', 'IsDeleted', '1', '0', '2', 'false');
INSERT INTO `InterfaceCondition` VALUES ('ca3606db-88a1-43bf-8bc0-d024db43b15f', 'ed78acb7-ab6b-4c0e-afa2-8a015685ea6b', '||', '', '', '0', '0', '0', '2020-06-29 05:02:10', '0', '2020-06-29 05:02:10', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'b02079e2-5d5e-46a0-a549-a17e25e81389', '00000000-0000-0000-0000-000000000000', '1', '00000000-0000-0000-0000-000000000000', '-1', '1', '2', '-1', '-1');
INSERT INTO `InterfaceCondition` VALUES ('d640e540-7dcb-4d9d-8aba-630bdcf81c97', '396e80d2-992a-4ac6-8e42-c6966002371d', 'Age == 90', '', '', '0', '0', '0', '2020-06-29 05:02:10', '0', '2020-06-29 05:02:10', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'b02079e2-5d5e-46a0-a549-a17e25e81389', 'ca3606db-88a1-43bf-8bc0-d024db43b15f', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '1', '0', '2', '90');
INSERT INTO `InterfaceCondition` VALUES ('db9de2c8-7665-4b02-9a20-f3c29b8c918f', '2f1c7caa-33e7-4dda-b95d-33aecb2050b7', 'Age > 10', '', '', '0', '0', '0', '2020-06-07 22:34:37', '0', '2020-06-07 22:34:37', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'b02079e2-5d5e-46a0-a549-a17e25e81389', '9a30e735-5c67-4dde-bbc2-4cfd310e199c', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '3', '0', '2', '10');
INSERT INTO `InterfaceCondition` VALUES ('e1356bf9-4fac-402c-8503-8453ba9e553e', 'SevenTinyTest.Reocord.Entry2020', '2020年入职的未删除人员', null, null, '0', '0', '0', '2020-06-07 22:38:12', '0', '2020-06-07 22:38:12', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '00000000-0000-0000-0000-000000000000', null, '0', '0', '0', null);
INSERT INTO `InterfaceCondition` VALUES ('eb74ecf7-cd96-400f-98ea-ef7eef1b3c2c', '96d1667f-14b1-4f14-acf4-6e4bd4964960', 'IsDeleted == false', '', '', '0', '0', '0', '2020-06-07 22:40:35', '0', '2020-06-07 22:40:35', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'e1356bf9-4fac-402c-8503-8453ba9e553e', 'f3fe067e-a9ef-4bc8-9fb0-d0dff046df0b', '2', 'ddac4581-376d-4407-83fb-2e77bf67a8bf', 'IsDeleted', '1', '0', '2', 'false');
INSERT INTO `InterfaceCondition` VALUES ('ec16d99c-e348-471f-8cfc-d6659daedc09', '103515ec-057d-4976-956b-7e76f729c4be', 'Age >= 90', '', '', '0', '0', '0', '2020-06-26 13:26:25', '0', '2020-06-26 13:26:25', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'bf3390a3-b11e-477c-9e08-d01bbcdef9ac', '0b05caf1-268d-4315-b554-90ae8f7d9e0f', '2', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '5', '0', '2', '90');
INSERT INTO `InterfaceCondition` VALUES ('f3fe067e-a9ef-4bc8-9fb0-d0dff046df0b', '852aec90-bda9-41b7-a344-f18e85949cc9', '&&', '', '', '0', '0', '0', '2020-06-07 22:39:13', '0', '2020-06-07 22:40:11', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'e1356bf9-4fac-402c-8503-8453ba9e553e', '00000000-0000-0000-0000-000000000000', '1', '00000000-0000-0000-0000-000000000000', '-1', '6', '1', '-1', '-1');

-- ----------------------------
-- Table structure for InterfaceFields
-- ----------------------------
DROP TABLE IF EXISTS `InterfaceFields`;
CREATE TABLE `InterfaceFields` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `MetaObjectId` char(36) NOT NULL,
  `CloudApplicationtId` char(36) NOT NULL,
  `MetaFieldId` char(36) NOT NULL,
  `MetaFieldShortCode` varchar(255) NOT NULL,
  `MetaFieldCustomViewName` varchar(255) DEFAULT NULL,
  `ParentId` char(36) NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of InterfaceFields
-- ----------------------------
INSERT INTO `InterfaceFields` VALUES ('0763ecf4-e88a-4c6a-a32e-f0b624fd9277', 'SevenTinyTest.UserInformation._id', '数据ID', '', '', '0', '0', '0', '2020-06-26 13:30:02', '0', '2020-06-26 13:30:02', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'd16753fa-1aa6-4ce0-98ac-d3aa219b2fb3', '_id', '数据ID', '2b87774b-cc9a-4eda-9c56-87046f2ec3d9');
INSERT INTO `InterfaceFields` VALUES ('0c911b26-f204-482c-b545-205d69b58720', 'SevenTinyTest.UserInformation.Age', '年龄', '', '', '0', '0', '0', '2020-06-07 10:30:46', '0', '2020-06-07 10:30:46', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '年龄', '2b87774b-cc9a-4eda-9c56-87046f2ec3d9');
INSERT INTO `InterfaceFields` VALUES ('1c9911a4-02a2-47d2-b0eb-0211039a62e4', 'SevenTinyTest.UserInformation.CreateBy', '创建人', '', '', '0', '0', '0', '2020-06-22 21:54:09', '0', '2020-06-22 21:54:09', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'cc85d85a-0715-4b01-805c-8b9beb45b86b', 'CreateBy', '创建人', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('1e547bc1-fbd6-45a7-af08-12147223337f', 'SevenTinyTest.UserInformation._id', '数据ID', '', '', '0', '0', '0', '2020-06-29 05:36:57', '0', '2020-06-29 05:36:57', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'd16753fa-1aa6-4ce0-98ac-d3aa219b2fb3', '_id', '数据ID', '350297d8-8e42-4ef0-87c1-7d3d783cd372');
INSERT INTO `InterfaceFields` VALUES ('243a0f55-00ad-4255-ad86-17303a3ae7a5', 'SevenTinyTest.UserInformation.Age', '年龄', '', '', '0', '0', '0', '2020-06-29 04:38:54', '0', '2020-06-29 04:38:54', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '年龄', '7d396895-c73b-4dec-b2d3-f2713e339473');
INSERT INTO `InterfaceFields` VALUES ('266faede-bdfd-4937-af6d-008860b921ca', 'SevenTinyTest.UserInformation._id', '数据ID', '', '', '0', '0', '0', '2020-06-08 23:15:38', '0', '2020-06-08 23:21:19', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'd16753fa-1aa6-4ce0-98ac-d3aa219b2fb3', '_id', 'ID', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('2b47108b-8711-4f15-950e-92127a0d5eca', 'SevenTinyTest.UserInformation.Age', '年龄', '', '', '0', '0', '0', '2020-06-29 05:36:57', '0', '2020-06-29 05:36:57', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '年龄', '350297d8-8e42-4ef0-87c1-7d3d783cd372');
INSERT INTO `InterfaceFields` VALUES ('2b87774b-cc9a-4eda-9c56-87046f2ec3d9', 'SevenTinyTest.UserInformation.Default2', '业务字段', null, null, '0', '0', '0', '2020-06-06 23:46:59', '0', '2020-06-25 01:55:25', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '-', null, '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceFields` VALUES ('2f9d5bf5-9d1c-455c-9154-913a8a7c082f', 'SevenTinyTest.UserInformation.Tel', '手机', '', '', '0', '0', '0', '2020-06-29 05:36:57', '0', '2020-06-29 05:36:57', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', 'b45ec90f-e00a-4edc-89c4-5f23c8d96e7b', 'Tel', '手机', '350297d8-8e42-4ef0-87c1-7d3d783cd372');
INSERT INTO `InterfaceFields` VALUES ('350297d8-8e42-4ef0-87c1-7d3d783cd372', 'SevenTinyTest.UserInformation.Info', '员工信息', null, null, '0', '0', '0', '2020-06-29 05:36:33', '0', '2020-06-29 05:36:33', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '-', null, '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceFields` VALUES ('3b40a155-3c09-4c48-b398-c38786081f53', 'SevenTinyTest.UserInformation.Age', '年龄', '', '', '0', '0', '0', '2020-06-22 21:03:34', '0', '2020-06-22 21:03:34', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '年龄', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('4cf364ff-6611-485b-8486-e32bf87f4fca', 'SevenTinyTest.UserInformation.ModifyBy', '修改人', '', '', '0', '0', '0', '2020-06-22 21:54:09', '0', '2020-06-22 21:54:09', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '2b042096-8f8c-47b0-8d7f-eb6ea95601c8', 'ModifyBy', '修改人', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('591cb664-c316-45f3-aff9-827668a7e8e8', 'SevenTinyTest.Reocord.IsDeleted', '是否删除', '', '', '0', '0', '0', '2020-06-07 22:44:07', '0', '2020-06-07 22:44:07', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'ddac4581-376d-4407-83fb-2e77bf67a8bf', 'IsDeleted', '是否删除', 'a23b9cb4-e2d0-4671-a8e7-5ce95ce6173b');
INSERT INTO `InterfaceFields` VALUES ('5a058766-b773-4e59-a2f6-4e4906a19b6f', 'SevenTinyTest.UserInformation.CreateTime', '创建时间', '', '', '0', '0', '0', '2020-06-22 21:54:09', '0', '2020-06-22 21:54:09', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1189e588-a429-49d9-aae4-5c6b3e90596a', 'CreateTime', '创建时间', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('6818a1a1-296f-43e4-834f-5de40213d7a1', 'SevenTinyTest.Reocord.EndTime', '结束时间', '', '', '0', '0', '0', '2020-06-07 22:44:07', '0', '2020-06-07 22:44:07', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', '00000000-0000-0000-0000-000000000000', 'a2cbb891-7573-4ca8-95d5-f9f932bdd39f', 'EndTime', '结束时间', 'a23b9cb4-e2d0-4671-a8e7-5ce95ce6173b');
INSERT INTO `InterfaceFields` VALUES ('7782079c-4144-44b5-ab03-d75de4e8053e', 'SevenTinyTest.UserInformation.IsDeleted', '是否删除', '', '', '0', '0', '0', '2020-06-22 21:54:09', '0', '2020-06-22 21:54:09', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'd5f9b763-ddd2-4ea2-bfb4-eaecb98ce41e', 'IsDeleted', '是否删除', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('7875e242-2e9d-4e4f-a160-072ba2011341', 'SevenTinyTest.Reocord.StartTime', '开始时间', '', '', '0', '0', '0', '2020-06-07 22:44:07', '0', '2020-06-07 22:44:07', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', '00000000-0000-0000-0000-000000000000', '02c57ac8-3b04-40e4-9e28-470f62aef858', 'StartTime', '开始时间', 'a23b9cb4-e2d0-4671-a8e7-5ce95ce6173b');
INSERT INTO `InterfaceFields` VALUES ('7d396895-c73b-4dec-b2d3-f2713e339473', 'SevenTinyTest.UserInformation.AgeNameList', '姓名年龄', null, null, '0', '0', '0', '2020-06-29 04:38:45', '0', '2020-06-29 04:38:45', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '-', null, '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceFields` VALUES ('8573e280-bdc3-4fdd-b08d-365693782479', 'SevenTinyTest.UserInformation.Default', '全部', null, null, '0', '0', '0', '2020-06-06 23:46:27', '0', '2020-06-22 21:54:45', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '-', null, '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceFields` VALUES ('918b1b75-8505-4004-9b75-0db15984b971', 'SevenTinyTest.UserInformation.Name', '姓名', '', '', '0', '0', '0', '2020-06-07 10:30:46', '0', '2020-06-07 10:30:54', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'Name', '员工姓名', '2b87774b-cc9a-4eda-9c56-87046f2ec3d9');
INSERT INTO `InterfaceFields` VALUES ('a23b9cb4-e2d0-4671-a8e7-5ce95ce6173b', 'SevenTinyTest.Reocord.Default', '默认列表', null, null, '0', '0', '0', '2020-06-07 22:43:57', '0', '2020-06-07 22:43:57', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '-', null, '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceFields` VALUES ('a37cbcb6-5d28-48fa-8d45-3327cc4d8f1d', 'SevenTinyTest.UserInformation.ModifyTime', '修改时间', '', '', '0', '0', '0', '2020-06-22 21:54:09', '0', '2020-06-22 21:54:09', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '73fe5759-73fa-405f-aeb4-c94d41f30594', 'ModifyTime', '修改时间', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('c64dd091-775c-4984-a331-b5897148a75f', 'SevenTinyTest.UserInformation.Name', '姓名', '', '', '0', '0', '0', '2020-06-29 05:36:57', '0', '2020-06-29 05:36:57', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'Name', '姓名', '350297d8-8e42-4ef0-87c1-7d3d783cd372');
INSERT INTO `InterfaceFields` VALUES ('cb59f192-6270-4066-af64-45c92b65cd7d', 'SevenTinyTest.UserInformation.Name', '姓名', '', '', '0', '0', '0', '2020-06-22 21:54:09', '0', '2020-06-22 21:54:09', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'Name', '姓名', '8573e280-bdc3-4fdd-b08d-365693782479');
INSERT INTO `InterfaceFields` VALUES ('fb855953-39c7-4c1c-bee9-572c5d14d9c6', 'SevenTinyTest.UserInformation.Name', '姓名', '', '', '0', '0', '0', '2020-06-29 04:38:54', '0', '2020-06-29 04:38:54', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'Name', '姓名', '7d396895-c73b-4dec-b2d3-f2713e339473');

-- ----------------------------
-- Table structure for InterfaceSetting
-- ----------------------------
DROP TABLE IF EXISTS `InterfaceSetting`;
CREATE TABLE `InterfaceSetting` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `MetaObjectId` char(36) NOT NULL,
  `MetaObjectCode` varchar(255) NOT NULL,
  `CloudApplicationtId` char(36) NOT NULL,
  `CloudApplicationCode` varchar(255) NOT NULL,
  `InterfaceType` int NOT NULL,
  `InterfaceConditionId` char(36) NOT NULL,
  `InterfaceVerificationId` char(36) NOT NULL,
  `InterfaceFieldsId` char(36) NOT NULL,
  `InterfaceSortId` char(36) NOT NULL,
  `PageSize` int NOT NULL,
  `DataSousrceId` char(36) NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of InterfaceSetting
-- ----------------------------
INSERT INTO `InterfaceSetting` VALUES ('02e4f033-e6fc-4152-9cb9-206d48da486c', 'SevenTinyTest.TestScript', 'TestScript', '', '', '0', '1', '0', '2020-06-26 22:16:16', '0', '2020-06-26 22:16:16', '0', '00000000-0000-0000-0000-000000000000', '-', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '8', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', 'aa2efce4-e294-43a6-b112-f70c3433c578');
INSERT INTO `InterfaceSetting` VALUES ('138efbd7-16c6-4d41-b20e-5ed79b749aee', 'SevenTinyTest.UserInformation.UnDeletedAndAgeRatherThanFromArg', '未删除且年龄大于参数列表', null, null, '0', '0', '0', '2020-06-27 22:17:35', '0', '2020-06-27 22:17:35', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '7', '385e8721-4634-44e5-8870-bb485fcd9624', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '2b87774b-cc9a-4eda-9c56-87046f2ec3d9', '00000000-0000-0000-0000-000000000000', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('3149bb52-ef9a-45f6-a98c-a3f265bb9f38', 'SevenTinyTest.TestScript', 'TestScript', '', '', '0', '0', '0', '2020-06-27 12:40:53', '0', '2020-06-27 12:40:53', '0', '00000000-0000-0000-0000-000000000000', '-', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '8', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '2ddc43b0-0bba-403b-86f3-ddc0f0fd87b1');
INSERT INTO `InterfaceSetting` VALUES ('320d7fd0-7b5f-4130-83d2-e6bfc9480c29', 'SevenTinyTest.TestScript', 'TestScript', '', '', '0', '1', '0', '2020-06-27 12:33:28', '0', '2020-06-27 12:33:28', '0', '00000000-0000-0000-0000-000000000000', '-', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '8', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', 'e72ef629-20b0-4dce-af73-73270ad2a601');
INSERT INTO `InterfaceSetting` VALUES ('382802cc-a640-455c-a010-6e33f9515935', 'SevenTinyTest.TestJson', 'TestJson', '', '', '0', '1', '0', '2020-06-26 20:52:44', '0', '2020-06-26 20:52:44', '0', '00000000-0000-0000-0000-000000000000', '-', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '9', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '87af6209-5180-4fe1-bbf2-27c405dc39b9');
INSERT INTO `InterfaceSetting` VALUES ('42f06fc2-c5c1-4717-b43d-4f796b2111bb', 'SevenTinyTest.UserInformation.AgeRatherThan95Interface', '年龄大于95的人', null, null, '0', '0', '0', '2020-06-29 04:41:01', '0', '2020-06-29 10:36:28', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '7', '86b117d0-a668-4fa5-83ee-ce560de2a7eb', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '7d396895-c73b-4dec-b2d3-f2713e339473', 'e71c0726-d6d4-436d-8216-d700225b061f', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('55388b9c-7907-406f-ba43-477dfa7d866d', 'SevenTinyTest.UserInformation.AgeLargeArg', '年龄大于参数', null, null, '0', '0', '0', '2020-06-29 04:54:21', '0', '2020-06-29 04:54:47', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '7', '385e8721-4634-44e5-8870-bb485fcd9624', '00000000-0000-0000-0000-000000000000', '7d396895-c73b-4dec-b2d3-f2713e339473', 'e71c0726-d6d4-436d-8216-d700225b061f', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('60298013-ac57-45df-9d64-20e65d0e0ae7', 'SevenTinyTest.UserInformation.AgeRatherThan90', '查年龄大于90的人', null, null, '0', '0', '0', '2020-06-27 22:17:37', '0', '2020-06-27 22:17:37', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '7', 'bf3390a3-b11e-477c-9e08-d01bbcdef9ac', '00000000-0000-0000-0000-000000000000', '2b87774b-cc9a-4eda-9c56-87046f2ec3d9', 'e71c0726-d6d4-436d-8216-d700225b061f', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('6bb0d0c9-2f7a-4e89-ba06-896525092dc6', 'SevenTinyTest.UserInformation.InfoAge', '查询年龄', null, null, '0', '0', '0', '2020-06-29 05:51:01', '0', '2020-06-29 05:51:01', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '7', 'a1a69a53-7bc2-406f-acf9-207f387ccf5b', '3c6aa52d-e5db-42b7-9954-edb0e23684a9', '350297d8-8e42-4ef0-87c1-7d3d783cd372', '30836d12-e6e9-4256-acea-ba84982e3637', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('72e33a2f-beff-44f0-a4c1-f5a20777c656', 'SevenTinyTest.UserInformation.AddOne', '新增一个人', null, null, '0', '0', '0', '2020-06-29 04:47:13', '0', '2020-06-29 04:50:30', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '1', '00000000-0000-0000-0000-000000000000', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '8573e280-bdc3-4fdd-b08d-365693782479', '00000000-0000-0000-0000-000000000000', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('b6990730-094b-456c-8442-5ff67bc6d13a', 'SevenTinyTest.UserInformation.UpdateNameByAge', 'UpdateNameByAge', null, null, '0', '0', '0', '2020-06-27 22:17:39', '0', '2020-06-27 22:17:39', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '3', 'a4284376-6331-47ca-be6c-b51f89869a42', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('bc3b9a77-a20e-4809-ac33-b640d186acf5', 'SevenTinyTest.Test2', 'Test2', '', '', '0', '1', '0', '2020-06-26 20:51:16', '0', '2020-06-26 20:51:16', '0', '00000000-0000-0000-0000-000000000000', '-', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '8', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '87d837aa-82c5-4b81-a475-f7204f9836d6');
INSERT INTO `InterfaceSetting` VALUES ('bc8a2b92-895f-4ab0-b477-0469b9052972', 'SevenTinyTest.TestScript', '测试脚本', '', '', '0', '1', '0', '2020-06-26 21:01:33', '0', '2020-06-26 21:01:33', '0', '00000000-0000-0000-0000-000000000000', '-', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '8', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', 'e93ba449-d2db-471e-8953-263b4ce46949');
INSERT INTO `InterfaceSetting` VALUES ('c2a9fa4d-f40a-4575-be3b-c65665ef01f9', 'SevenTinyTest.UserInformation.UnDeletedAndAgeRatherThanFromArgCount', '未删除且年龄大于参数Count', null, null, '0', '0', '0', '2020-06-27 22:17:40', '0', '2020-06-27 22:17:40', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '5', '385e8721-4634-44e5-8870-bb485fcd9624', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('c7215c9d-d896-4ba0-a426-ff7fc431db16', 'SevenTinyTest.UserInformation.AgeRatherThan10Count', '年龄大于10的数量', null, null, '0', '0', '0', '2020-06-27 22:17:41', '0', '2020-06-27 22:17:41', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '5', 'b02079e2-5d5e-46a0-a549-a17e25e81389', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('ce221249-9e44-4890-bbf7-39bee7c57204', 'SevenTinyTest.UserInformation.UndeletedList', '未删除列表', null, null, '0', '0', '0', '2020-06-27 22:17:42', '0', '2020-06-27 22:17:42', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '7', 'bf3390a3-b11e-477c-9e08-d01bbcdef9ac', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '8573e280-bdc3-4fdd-b08d-365693782479', '00000000-0000-0000-0000-000000000000', '30', '00000000-0000-0000-0000-000000000000');
INSERT INTO `InterfaceSetting` VALUES ('db387760-7ded-4e57-99eb-a3ff876ab1b8', 'SevenTinyTest.TestJson', 'TestJson', '', '', '0', '0', '0', '2020-06-26 21:01:17', '0', '2020-06-26 21:01:17', '0', '00000000-0000-0000-0000-000000000000', '-', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '9', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '0', '4ecf6bda-e7c0-4ed0-b833-7b3788c59cf4');
INSERT INTO `InterfaceSetting` VALUES ('dc189ba2-9f10-4662-9716-c20866bc1113', 'SevenTinyTest.UserInformation.AddUserInformation', '添加人员信息', null, null, '0', '0', '0', '2020-06-29 00:45:04', '0', '2020-06-29 00:45:04', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'SevenTinyTest', '2', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '8573e280-bdc3-4fdd-b08d-365693782479', '00000000-0000-0000-0000-000000000000', '30', '00000000-0000-0000-0000-000000000000');

-- ----------------------------
-- Table structure for InterfaceSort
-- ----------------------------
DROP TABLE IF EXISTS `InterfaceSort`;
CREATE TABLE `InterfaceSort` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `MetaObjectId` char(36) NOT NULL,
  `CloudApplicationtId` char(36) NOT NULL,
  `ParentId` char(36) NOT NULL,
  `MetaFieldId` char(36) NOT NULL,
  `MetaFieldShortCode` varchar(255) NOT NULL,
  `SortType` int NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of InterfaceSort
-- ----------------------------
INSERT INTO `InterfaceSort` VALUES ('30836d12-e6e9-4256-acea-ba84982e3637', 'SevenTinyTest.UserInformation.AgeAsc', '年龄顺序', null, null, '0', '0', '0', '2020-06-29 04:40:18', '0', '2020-06-29 04:40:18', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '4c6ffd25814d4903b8c652cd3dfa1192', '0');
INSERT INTO `InterfaceSort` VALUES ('6e2ef213-ac41-48c7-9c35-2b0056bd129d', '8348576194ed4c7d87f9544c12e0dd6a', '年龄', '', '', '0', '0', '0', '2020-06-26 12:58:46', '0', '2020-06-29 10:37:29', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', 'e71c0726-d6d4-436d-8216-d700225b061f', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '0');
INSERT INTO `InterfaceSort` VALUES ('e71c0726-d6d4-436d-8216-d700225b061f', 'SevenTinyTest.UserInformation.AgeDesc', '年龄倒叙', null, null, '0', '0', '0', '2020-06-26 12:53:00', '0', '2020-06-26 12:53:36', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'c09d31bbbc32405ba442ea519fce5a54', '0');
INSERT INTO `InterfaceSort` VALUES ('ededabe8-aa90-485b-badc-4b81997f0fcf', '5f71a675d4564454a57505767a59c485', '年龄', '', '', '0', '0', '0', '2020-06-29 04:40:24', '0', '2020-06-29 04:40:24', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '30836d12-e6e9-4256-acea-ba84982e3637', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '1');

-- ----------------------------
-- Table structure for InterfaceVerification
-- ----------------------------
DROP TABLE IF EXISTS `InterfaceVerification`;
CREATE TABLE `InterfaceVerification` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `MetaObjectId` char(36) NOT NULL,
  `CloudApplicationtId` char(36) NOT NULL,
  `ParentId` char(36) NOT NULL,
  `MetaFieldId` char(36) NOT NULL,
  `MetaFieldShortCode` varchar(255) NOT NULL,
  `RegularExpression` varchar(255) DEFAULT NULL,
  `VerificationTips` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of InterfaceVerification
-- ----------------------------
INSERT INTO `InterfaceVerification` VALUES ('12ecca85-1fbc-44da-ad0d-21f2d3910984', '66423ee7-1927-4f37-b099-00d0d13aa4d7', '姓名', '', '', '0', '1', '0', '2020-06-22 23:38:46', '0', '2020-06-22 23:38:46', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'Name', '^\\d{1,2}$', '3333');
INSERT INTO `InterfaceVerification` VALUES ('14ef20d4-3013-4460-b43a-0a66c33d83d1', 'SevenTinyTest.UserInformation.Age', '年龄', '', '', '0', '0', '0', '2020-06-08 22:06:39', '0', '2020-06-08 22:06:39', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', 'b63cfc66-c8af-4df8-8566-00556e4cb309', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', null, null);
INSERT INTO `InterfaceVerification` VALUES ('380813d0-80a0-4f88-b828-7854dde6c167', '8a0dc2ab-a622-4378-ad7a-3eb030535159', '年龄', '', '', '0', '0', '0', '2020-06-29 05:50:06', '0', '2020-06-29 05:50:06', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '3c6aa52d-e5db-42b7-9954-edb0e23684a9', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '^\\d{1,2}$', '年龄输入错误');
INSERT INTO `InterfaceVerification` VALUES ('3c6aa52d-e5db-42b7-9954-edb0e23684a9', 'SevenTinyTest.UserInformation.InfoInterfaceVerify', '员工信息接口校验', null, null, '0', '0', '0', '2020-06-29 05:40:29', '0', '2020-06-29 05:40:29', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '-', null, null);
INSERT INTO `InterfaceVerification` VALUES ('4439a1b4-ca70-44ca-b824-8ed78f80d7ef', 'SevenTinyTest.UserInformation.Check1', '校验年龄', null, null, '0', '0', '0', '2020-06-08 21:49:20', '0', '2020-06-22 22:04:34', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '-', null, null);
INSERT INTO `InterfaceVerification` VALUES ('6ac0f71f-f7fa-4ed7-9286-665950592214', '4ea5a369-3814-4657-95f9-4edcd8f035b9', '修改人', '', '', '0', '1', '0', '2020-06-22 23:49:54', '0', '2020-06-22 23:49:54', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '2b042096-8f8c-47b0-8d7f-eb6ea95601c8', 'ModifyBy', '^\\d{1,2}$', '33333');
INSERT INTO `InterfaceVerification` VALUES ('84644ab1-b583-446e-ad80-a1e202fb489d', '-', '-', '', '', '0', '1', '0', '2020-06-22 23:34:59', '0', '2020-06-22 23:34:59', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '3333', '444');
INSERT INTO `InterfaceVerification` VALUES ('a670c423-d1b4-4d9c-813c-68aa3f7588e9', '28a7ab54-4801-4633-904d-16e902e1676a', '姓名', '', '', '0', '1', '0', '2020-06-22 23:46:27', '0', '2020-06-22 23:46:27', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'Name', '3344', '444');
INSERT INTO `InterfaceVerification` VALUES ('b63cfc66-c8af-4df8-8566-00556e4cb309', 'SevenTinyTest.UserInformation.Verify1', '接口校验一', null, null, '0', '0', '0', '2020-06-08 21:49:39', '0', '2020-06-08 21:49:39', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '-', null, null);
INSERT INTO `InterfaceVerification` VALUES ('bf5200fb-6a2c-4390-80f3-6ffce408abe3', 'SevenTinyTest.UserInformation.IsDeleted', '是否删除', '', '', '0', '1', '0', '2020-06-08 22:04:34', '0', '2020-06-08 22:04:34', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', 'd5f9b763-ddd2-4ea2-bfb4-eaecb98ce41e', 'IsDeleted', null, null);
INSERT INTO `InterfaceVerification` VALUES ('c9154c7a-b5d5-4bde-a662-d6126c25ae49', 'SevenTinyTest.UserInformation.Age', '年龄', '', '', '0', '0', '0', '2020-06-08 23:30:17', '0', '2020-06-22 23:51:02', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'Age', '^\\d{1,2}$', '你这是年龄？');
INSERT INTO `InterfaceVerification` VALUES ('d81adf47-8d9b-4189-a7f0-f458fc77f1de', 'SevenTinyTest.UserInformation.Name', '姓名', '', '', '0', '1', '0', '2020-06-08 22:04:34', '0', '2020-06-08 22:04:34', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000', '4439a1b4-ca70-44ca-b824-8ed78f80d7ef', '4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'Name', null, null);

-- ----------------------------
-- Table structure for MetaField
-- ----------------------------
DROP TABLE IF EXISTS `MetaField`;
CREATE TABLE `MetaField` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `ShortCode` varchar(255) NOT NULL,
  `FieldType` int NOT NULL,
  `DataSourceId` char(36) DEFAULT NULL,
  `IsSystem` int NOT NULL,
  `MetaObjectId` char(36) NOT NULL,
  `CloudApplicationtId` char(36) NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of MetaField
-- ----------------------------
INSERT INTO `MetaField` VALUES ('01fe8a92-5cc3-4d20-ac79-60d398e65569', 'SevenTinyTest.Attendence.ModifyTime', '修改时间', '系统字段', '系统', '-1', '0', '0', '2020-06-29 04:59:46', '0', '2020-06-29 04:59:46', '0', 'ModifyTime', '3', '00000000-0000-0000-0000-000000000000', '1', '7449491d-beb5-4ec8-b210-240d723b2283', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('02bfd9b2-15c7-48f8-8215-dca5a25e6ce4', 'SevenTinyTest.UndeletedList.CreateBy', '创建人', '系统字段', '系统', '-1', '0', '0', '2020-06-08 23:06:37', '0', '2020-06-08 23:06:37', '0', 'CreateBy', '6', '00000000-0000-0000-0000-000000000000', '1', 'a15af61e-ffcb-4887-9f37-d84b38ce38cd', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('02c57ac8-3b04-40e4-9e28-470f62aef858', 'SevenTinyTest.Reocord.StartTime', '开始时间', null, null, '0', '0', '0', '2020-06-07 22:37:21', '0', '2020-06-07 22:37:21', '0', 'StartTime', '3', '00000000-0000-0000-0000-000000000000', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', '00000000-0000-0000-0000-000000000000');
INSERT INTO `MetaField` VALUES ('0389daf5-5941-46fa-b91e-6ba1bfd6c9e4', 'SevenTinyTest.UndeletedList.ModifyBy', '修改人', '系统字段', '系统', '-1', '0', '0', '2020-06-08 23:06:37', '0', '2020-06-08 23:06:37', '0', 'ModifyBy', '6', '00000000-0000-0000-0000-000000000000', '1', 'a15af61e-ffcb-4887-9f37-d84b38ce38cd', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('1189e588-a429-49d9-aae4-5c6b3e90596a', 'SevenTinyTest.UserInformation.CreateTime', '创建时间', '系统字段', '系统', '-1', '0', '0', '2020-06-06 22:42:01', '0', '2020-06-06 22:42:01', '0', 'CreateTime', '3', '00000000-0000-0000-0000-000000000000', '1', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('241b30d4-6dc4-4d78-b08f-b02cd8ea1934', 'SevenTinyTest.SalaryProfile.IsDeleted', '是否删除', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:35:41', '0', '2020-06-07 22:35:41', '0', 'IsDeleted', '5', '00000000-0000-0000-0000-000000000000', '1', 'e3306884-90d7-465b-9034-a5ce57f10b46', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('2b042096-8f8c-47b0-8d7f-eb6ea95601c8', 'SevenTinyTest.UserInformation.ModifyBy', '修改人', '系统字段', '系统', '-1', '0', '0', '2020-06-06 22:42:01', '0', '2020-06-06 22:42:01', '0', 'ModifyBy', '6', '00000000-0000-0000-0000-000000000000', '1', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('2ce53251-baa5-456e-bfcf-7a78fa1d6b7b', 'SevenTinyTest.Attendence.ModifyBy', '修改人', '系统字段', '系统', '-1', '0', '0', '2020-06-29 04:59:46', '0', '2020-06-29 04:59:46', '0', 'ModifyBy', '6', '00000000-0000-0000-0000-000000000000', '1', '7449491d-beb5-4ec8-b210-240d723b2283', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('3e0be0ad-3371-4854-abba-a5c5c7d7fca9', 'SevenTinyTest.Verify1.CreateTime', '创建时间', '系统字段', '系统', '-1', '0', '0', '2020-06-08 20:58:10', '0', '2020-06-08 20:58:10', '0', 'CreateTime', '3', '00000000-0000-0000-0000-000000000000', '1', 'dde7f48e-920b-44e9-ba82-e301aadfaf29', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('4772169b-9227-4af4-9d35-4976f04f9b58', 'SevenTinyTest.SalaryProfile._id', '数据ID', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:35:41', '0', '2020-06-07 22:35:41', '0', '_id', '2', '00000000-0000-0000-0000-000000000000', '1', 'e3306884-90d7-465b-9034-a5ce57f10b46', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('4a3649fb-e843-4205-a5f2-a94cc493c884', 'SevenTinyTest.Attendence.CreateBy', '创建人', '系统字段', '系统', '-1', '0', '0', '2020-06-29 04:59:46', '0', '2020-06-29 04:59:46', '0', 'CreateBy', '6', '00000000-0000-0000-0000-000000000000', '1', '7449491d-beb5-4ec8-b210-240d723b2283', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('4de08b5d-4a7e-4d1a-8d46-c76d04b4dd55', 'SevenTinyTest.UserInformation.Name', '姓名', null, null, '0', '0', '0', '2020-06-07 10:29:59', '0', '2020-06-07 10:29:59', '0', 'Name', '2', '00000000-0000-0000-0000-000000000000', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000');
INSERT INTO `MetaField` VALUES ('54a9a81d-5ea3-4a79-bc9a-a4e5271f0876', 'SevenTinyTest.Verify1.IsDeleted', '是否删除', '系统字段', '系统', '-1', '0', '0', '2020-06-08 20:58:10', '0', '2020-06-08 20:58:10', '0', 'IsDeleted', '5', '00000000-0000-0000-0000-000000000000', '1', 'dde7f48e-920b-44e9-ba82-e301aadfaf29', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('609af886-0191-4e41-b2b4-eca4782835d3', 'SevenTinyTest.SalaryProfile.ModifyTime', '修改时间', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:35:41', '0', '2020-06-07 22:35:41', '0', 'ModifyTime', '3', '00000000-0000-0000-0000-000000000000', '1', 'e3306884-90d7-465b-9034-a5ce57f10b46', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('6147e37c-590b-4972-ad8c-cf79f972fb89', 'SevenTinyTest.UndeletedList.ModifyTime', '修改时间', '系统字段', '系统', '-1', '0', '0', '2020-06-08 23:06:37', '0', '2020-06-08 23:06:37', '0', 'ModifyTime', '3', '00000000-0000-0000-0000-000000000000', '1', 'a15af61e-ffcb-4887-9f37-d84b38ce38cd', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('618208d7-2122-416e-b64b-701668881bc7', 'SevenTinyTest.SalaryProfile.CreateBy', '创建人', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:35:41', '0', '2020-06-07 22:35:41', '0', 'CreateBy', '6', '00000000-0000-0000-0000-000000000000', '1', 'e3306884-90d7-465b-9034-a5ce57f10b46', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('6c9760b2-4d88-4bcd-b4f8-bc61870d994d', 'SevenTinyTest.UserInformation.Age', '年龄', null, null, '0', '0', '0', '2020-06-07 10:30:13', '0', '2020-06-07 10:30:13', '0', 'Age', '6', '00000000-0000-0000-0000-000000000000', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000');
INSERT INTO `MetaField` VALUES ('6f3d7cdf-f1a0-43a2-832f-d55f5df57402', 'SevenTinyTest.SalaryProfile.ModifyBy', '修改人', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:35:41', '0', '2020-06-07 22:35:41', '0', 'ModifyBy', '6', '00000000-0000-0000-0000-000000000000', '1', 'e3306884-90d7-465b-9034-a5ce57f10b46', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('73fe5759-73fa-405f-aeb4-c94d41f30594', 'SevenTinyTest.UserInformation.ModifyTime', '修改时间', '系统字段', '系统', '-1', '0', '0', '2020-06-06 22:42:01', '0', '2020-06-06 22:42:01', '0', 'ModifyTime', '3', '00000000-0000-0000-0000-000000000000', '1', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('7edba729-b4d2-4437-98bb-f1b1ca4cd02b', 'SevenTinyTest.Attendence._id', '数据ID', '系统字段', '系统', '-1', '0', '0', '2020-06-29 04:59:46', '0', '2020-06-29 04:59:46', '0', '_id', '2', '00000000-0000-0000-0000-000000000000', '1', '7449491d-beb5-4ec8-b210-240d723b2283', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('7f6f05c7-6f70-4deb-8a2e-e9b47dd3c109', 'SevenTinyTest.Reocord.ModifyBy', '修改人', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:37:06', '0', '2020-06-07 22:37:06', '0', 'ModifyBy', '6', '00000000-0000-0000-0000-000000000000', '1', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('9188b578-3296-4f42-a525-2a7b93e3bd45', 'SevenTinyTest.Attendence.CreateTime', '创建时间', '系统字段', '系统', '-1', '0', '0', '2020-06-29 04:59:46', '0', '2020-06-29 04:59:46', '0', 'CreateTime', '3', '00000000-0000-0000-0000-000000000000', '1', '7449491d-beb5-4ec8-b210-240d723b2283', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('9f02082a-550f-4b84-8838-d6d2eb3ccf94', 'SevenTinyTest.Reocord._id', '数据ID', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:37:06', '0', '2020-06-07 22:37:06', '0', '_id', '2', '00000000-0000-0000-0000-000000000000', '1', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('a2cbb891-7573-4ca8-95d5-f9f932bdd39f', 'SevenTinyTest.Reocord.EndTime', '结束时间', null, null, '0', '0', '0', '2020-06-07 22:37:34', '0', '2020-06-07 22:37:34', '0', 'EndTime', '3', '00000000-0000-0000-0000-000000000000', '0', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', '00000000-0000-0000-0000-000000000000');
INSERT INTO `MetaField` VALUES ('a5939e5f-ddd3-46c4-aa9e-797603df2a13', 'SevenTinyTest.Verify1.CreateBy', '创建人', '系统字段', '系统', '-1', '0', '0', '2020-06-08 20:58:10', '0', '2020-06-08 20:58:10', '0', 'CreateBy', '6', '00000000-0000-0000-0000-000000000000', '1', 'dde7f48e-920b-44e9-ba82-e301aadfaf29', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('a6fd1d6f-25d0-442b-9e2c-c31d37768f38', 'SevenTinyTest.Reocord.CreateBy', '创建人', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:37:06', '0', '2020-06-07 22:37:06', '0', 'CreateBy', '6', '00000000-0000-0000-0000-000000000000', '1', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('a89bf82b-e03c-4b59-91fc-10e4ec281f06', 'SevenTinyTest.Attendence.IsDeleted', '是否删除', '系统字段', '系统', '-1', '0', '0', '2020-06-29 04:59:46', '0', '2020-06-29 04:59:46', '0', 'IsDeleted', '5', '00000000-0000-0000-0000-000000000000', '1', '7449491d-beb5-4ec8-b210-240d723b2283', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('b45ec90f-e00a-4edc-89c4-5f23c8d96e7b', 'SevenTinyTest.UserInformation.Tel', '手机', null, null, '0', '0', '0', '2020-06-29 05:34:22', '0', '2020-06-29 05:34:22', '0', 'Tel', '2', '00000000-0000-0000-0000-000000000000', '0', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '00000000-0000-0000-0000-000000000000');
INSERT INTO `MetaField` VALUES ('c1d45292-3bd5-4107-a10a-0049a4c8740c', 'SevenTinyTest.UndeletedList.CreateTime', '创建时间', '系统字段', '系统', '-1', '0', '0', '2020-06-08 23:06:37', '0', '2020-06-08 23:06:37', '0', 'CreateTime', '3', '00000000-0000-0000-0000-000000000000', '1', 'a15af61e-ffcb-4887-9f37-d84b38ce38cd', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('cc85d85a-0715-4b01-805c-8b9beb45b86b', 'SevenTinyTest.UserInformation.CreateBy', '创建人', '系统字段', '系统', '-1', '0', '0', '2020-06-06 22:42:01', '0', '2020-06-06 22:42:01', '0', 'CreateBy', '6', '00000000-0000-0000-0000-000000000000', '1', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('d16753fa-1aa6-4ce0-98ac-d3aa219b2fb3', 'SevenTinyTest.UserInformation._id', '数据ID', '系统字段', '系统', '-1', '0', '0', '2020-06-06 22:42:01', '0', '2020-06-06 22:42:01', '0', '_id', '2', '00000000-0000-0000-0000-000000000000', '1', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('d1aebace-5e30-462e-8585-5bbf17c8263f', 'SevenTinyTest.Reocord.ModifyTime', '修改时间', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:37:06', '0', '2020-06-07 22:37:06', '0', 'ModifyTime', '3', '00000000-0000-0000-0000-000000000000', '1', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('d5f9b763-ddd2-4ea2-bfb4-eaecb98ce41e', 'SevenTinyTest.UserInformation.IsDeleted', '是否删除', '系统字段', '系统', '-1', '0', '0', '2020-06-06 22:42:01', '0', '2020-06-06 22:42:01', '0', 'IsDeleted', '5', '00000000-0000-0000-0000-000000000000', '1', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('ddac4581-376d-4407-83fb-2e77bf67a8bf', 'SevenTinyTest.Reocord.IsDeleted', '是否删除', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:37:06', '0', '2020-06-07 22:37:06', '0', 'IsDeleted', '5', '00000000-0000-0000-0000-000000000000', '1', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('e5ead1c3-5b2b-45f1-9af2-3f454edebef2', 'SevenTinyTest.Verify1._id', '数据ID', '系统字段', '系统', '-1', '0', '0', '2020-06-08 20:58:10', '0', '2020-06-08 20:58:10', '0', '_id', '2', '00000000-0000-0000-0000-000000000000', '1', 'dde7f48e-920b-44e9-ba82-e301aadfaf29', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('e7fa81d8-c71d-4ff8-a36e-297518501ebd', 'SevenTinyTest.UndeletedList._id', '数据ID', '系统字段', '系统', '-1', '0', '0', '2020-06-08 23:06:37', '0', '2020-06-08 23:06:37', '0', '_id', '2', '00000000-0000-0000-0000-000000000000', '1', 'a15af61e-ffcb-4887-9f37-d84b38ce38cd', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('f349ce71-2a2f-433d-ae1c-54facb46fed6', 'SevenTinyTest.Verify1.ModifyBy', '修改人', '系统字段', '系统', '-1', '0', '0', '2020-06-08 20:58:10', '0', '2020-06-08 20:58:10', '0', 'ModifyBy', '6', '00000000-0000-0000-0000-000000000000', '1', 'dde7f48e-920b-44e9-ba82-e301aadfaf29', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('f57b95de-3cc5-4e4a-ae43-5a9e43e877bd', 'SevenTinyTest.UndeletedList.IsDeleted', '是否删除', '系统字段', '系统', '-1', '0', '0', '2020-06-08 23:06:37', '0', '2020-06-08 23:06:37', '0', 'IsDeleted', '5', '00000000-0000-0000-0000-000000000000', '1', 'a15af61e-ffcb-4887-9f37-d84b38ce38cd', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('f977512b-d7e0-48d3-8c4f-7b42eb7e1995', 'SevenTinyTest.Reocord.CreateTime', '创建时间', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:37:06', '0', '2020-06-07 22:37:06', '0', 'CreateTime', '3', '00000000-0000-0000-0000-000000000000', '1', '05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('fd1c0ee0-b4cc-4969-86a0-788dc091a99c', 'SevenTinyTest.Verify1.ModifyTime', '修改时间', '系统字段', '系统', '-1', '0', '0', '2020-06-08 20:58:10', '0', '2020-06-08 20:58:10', '0', 'ModifyTime', '3', '00000000-0000-0000-0000-000000000000', '1', 'dde7f48e-920b-44e9-ba82-e301aadfaf29', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaField` VALUES ('ffd05f45-b936-44b2-b3cd-d8d9279cfca1', 'SevenTinyTest.SalaryProfile.CreateTime', '创建时间', '系统字段', '系统', '-1', '0', '0', '2020-06-07 22:35:41', '0', '2020-06-07 22:35:41', '0', 'CreateTime', '3', '00000000-0000-0000-0000-000000000000', '1', 'e3306884-90d7-465b-9034-a5ce57f10b46', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');

-- ----------------------------
-- Table structure for MetaObject
-- ----------------------------
DROP TABLE IF EXISTS `MetaObject`;
CREATE TABLE `MetaObject` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `CloudApplicationId` char(36) NOT NULL,
  PRIMARY KEY (`Id`,`Code`,`CloudApplicationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of MetaObject
-- ----------------------------
INSERT INTO `MetaObject` VALUES ('05606ad5-923e-4dd7-8e1d-f81e4d5044ce', 'SevenTinyTest.Reocord', '任职记录', null, null, '1', '0', '0', '2020-06-07 22:37:06', '0', '2020-06-27 13:51:02', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaObject` VALUES ('1ba123b9-a2e7-4602-b101-d9cc15a3925c', 'SevenTinyTest.UserInformation', '员工信息', '员工信息', '员工信息', '0', '0', '0', '2020-06-06 22:42:01', '0', '2020-06-06 22:42:19', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaObject` VALUES ('7449491d-beb5-4ec8-b210-240d723b2283', 'SevenTinyTest.Attendence', '假勤', null, null, '3', '0', '0', '2020-06-29 04:59:46', '0', '2020-06-29 05:00:00', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');
INSERT INTO `MetaObject` VALUES ('e3306884-90d7-465b-9034-a5ce57f10b46', 'SevenTinyTest.SalaryProfile', '薪资档案', null, null, '0', '0', '0', '2020-06-07 22:35:41', '0', '2020-06-07 22:35:41', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20');

-- ----------------------------
-- Table structure for Organization
-- ----------------------------
DROP TABLE IF EXISTS `Organization`;
CREATE TABLE `Organization` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `Parent` char(36) NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of Organization
-- ----------------------------

-- ----------------------------
-- Table structure for TriggerScript
-- ----------------------------
DROP TABLE IF EXISTS `TriggerScript`;
CREATE TABLE `TriggerScript` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `CloudApplicationId` char(36) NOT NULL,
  `MetaObjectId` char(36) NOT NULL,
  `ScriptType` int NOT NULL,
  `MetaObjectInterfaceServiceType` int NOT NULL,
  `Language` int NOT NULL,
  `ClassFullName` varchar(255) NOT NULL,
  `FunctionName` varchar(255) NOT NULL,
  `Script` text NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of TriggerScript
-- ----------------------------
INSERT INTO `TriggerScript` VALUES ('00826ab4-2444-4965-8d91-558d31f4f570', 'SevenTinyTest.UserInformation.QueryList_Before', '查询集合-前', null, null, '0', '1', '0', '2020-06-26 00:56:07', '0', '2020-06-26 00:56:16', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '1', '13', '1', 'MetaObjectInterfaceTrigger', 'QueryList_Before', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\n\r\npublic class MetaObjectInterfaceTrigger\r\n{\r\n    \r\n    public FilterDefinition<BsonDocument> QueryList_Before(Dictionary<string, object> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)\r\n    {\r\n        return filter;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('0c191b8a-f984-496b-8644-1944644fc1ad', 'SevenTinyTest.Test', '测试', null, null, '0', '1', '0', '2020-06-26 20:49:40', '0', '2020-06-26 20:49:40', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    public Result<object> Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n        return Result<object>.Success();\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('1b77e897-bb99-4393-bedd-ad0d5639168c', 'SevenTinyTest.UserInformation.QueryList_After', '查询集合-后', null, null, '0', '1', '0', '2020-06-26 00:56:01', '0', '2020-06-26 13:32:22', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '1', '14', '1', 'MetaObjectInterfaceTrigger', 'QueryList_After', 'using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\npublic class MetaObjectInterfaceTrigger\r\n{\r\n    public Result<List<Dictionary<string, CloudData>>> QueryList_After(Dictionary<string, object> triggerContext, Result<List<Dictionary<string, CloudData>>> result)\r\n    {\r\n        if(result.IsSuccess)\r\n        {\r\n            foreach (var item in result.Data)\r\n            {\r\n                if(item.ContainsKey(\"Age\"))\r\n                {\r\n                    item[\"Age\"].Text = $\"-----------年龄为:{item[\"Age\"].Value}岁-------------\";\r\n                }\r\n            }\r\n        }\r\n        return result;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('2ddc43b0-0bba-403b-86f3-ddc0f0fd87b1', 'SevenTinyTest.TestScript', 'TestScript', null, null, '0', '0', '0', '2020-06-27 12:40:52', '0', '2020-06-29 04:57:37', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\nusing Chameleon.Repository;\r\nusing Newtonsoft.Json;\r\nusing Microsoft.Extensions.Logging;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    //1. 记录日志\r\n    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();\r\n    /* \r\n        logger.LogDebug(string message, params object[] args);\r\n        logger.LogError(string message, params object[] args);\r\n        logger.LogInformation(string message, params object[] args);\r\n    */\r\n    //2. 查询数据\r\n    //MongoDb数据库查询上下文，需要时放开下行注释使用\r\n    ChameleonDataDbContext dbContext = new ChameleonDataDbContext();\r\n    /* \r\n     *  查询数据的模板\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"key\",\"value\"),bf.Eq(\"key2\",\"value2\"));\r\n        dbContext.GetCollectionBson(\"对象编码\").Find(filter);\r\n    */\r\n\r\n    public object Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n        logger.LogError($\"arg={JsonConvert.SerializeObject(argumentsUpperKeyDic)}\");\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"Age\",\"90\"),bf.Eq(\"key2\",\"value2\"));\r\n        var result = dbContext.GetCollectionBson(\"SevenTinyTest.UserInformation\").Find(bf.Eq(\"Age\",90)).ToList();\r\n        return result;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('31a00285-e120-4cae-837d-e3e759a4c8a3', 'SevenTinyTest.DefaultJson', 'DefaultJson', null, null, '0', '1', '0', '2020-06-26 20:37:34', '0', '2020-06-26 20:37:34', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '3', '0', '0', '-', '-', '{\r\n    \"Key\":\"item\"\r\n}');
INSERT INTO `TriggerScript` VALUES ('3dfe5318-b7ed-41e8-8223-5922a5ca3b88', 'SevenTinyTest.UserInformation.QueryList_After', '查询集合-后', null, null, '0', '1', '0', '2020-06-27 12:27:57', '0', '2020-06-27 12:32:29', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '1', '14', '1', 'MetaObjectInterfaceTrigger', 'QueryList_After', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\nusing Chameleon.Repository;\r\n\r\n\r\npublic class MetaObjectInterfaceTrigger\r\n{\r\n    \r\n    //记录日志\r\n    Microsoft.Extensions.Logging.ILogger logger = new SevenTiny.Bantina.Logging.LogManager();\r\n    //MongoDb数据库查询上下文，需要时放开下行注释使用\r\n    //ChameleonDataDbContext dbContext = new ChameleonDataDbContext();\r\n    /* \r\n     *  查询数据的模板\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"key\",\"value\"),bf.Eq(\"key2\",\"value2\"));\r\n        dbContext.GetCollectionBson(\"对象编码\").Find(filter);\r\n    */\r\n\r\n    public List<Dictionary<string, CloudData>> QueryList_After(Dictionary<string, object> triggerContext, List<Dictionary<string, CloudData>> result)\r\n    {\r\n        if(result==null)\r\n            return result;\r\n\r\n        foreach (var item in result)\r\n        {\r\n            if(item.ContainsKey(\"Age\"))\r\n            {\r\n                item[\"Age\"].Text = $\"-----------年龄为:{item[\"Age\"].Value}岁-------------\";\r\n            }\r\n        }\r\n        \r\n        return result;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('4c3b5f07-9b1b-423a-ada9-e89ee2c17fb4', 'SevenTinyTest.UserInformation.QueryList_After', '查询集合-后', null, null, '0', '0', '0', '2020-06-27 12:43:44', '0', '2020-06-30 07:04:45', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '1', '14', '1', 'MetaObjectInterfaceTrigger', 'QueryList_After', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\nusing Chameleon.Repository;\r\nusing Newtonsoft.Json;\r\nusing Microsoft.Extensions.Logging;\r\n\r\n\r\npublic class MetaObjectInterfaceTrigger\r\n{\r\n    \r\n    //1. 记录日志\r\n    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();\r\n    /* \r\n        logger.LogDebug(string message, params object[] args);\r\n        logger.LogError(string message, params object[] args);\r\n        logger.LogInformation(string message, params object[] args);\r\n    */\r\n    //2. 查询数据\r\n    //MongoDb数据库查询上下文，需要时放开下行注释使用\r\n    //ChameleonDataDbContext dbContext = new ChameleonDataDbContext();\r\n    /* \r\n     *  查询数据的模板\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"key\",\"value\"),bf.Eq(\"key2\",\"value2\"));\r\n        dbContext.GetCollectionBson(\"对象编码\").Find(filter);\r\n    */\r\n\r\n    public List<Dictionary<string, CloudData>> QueryList_After(Dictionary<string, object> triggerContext, List<Dictionary<string, CloudData>> result)\r\n    {\r\n        if(result==null)\r\n            return result;\r\n      \r\n      	//logger.LogDebug($\"context={JsonConvert.SerializeObject(triggerContext)}\");\r\n	\r\n      	if(triggerContext.ContainsKey(\"Interface\"))\r\n        {\r\n           if(triggerContext[\"Interface\"]==\"SevenTinyTest.UserInformation.AgeRatherThan95Interface\")\r\n           {\r\n                foreach (var item in result)\r\n                {\r\n                    if(item.ContainsKey(\"Age\"))\r\n                    {\r\n                        item[\"Age\"].Text = $\"-年龄为:{item[\"Age\"].Value}岁-\";\r\n                    }\r\n                }\r\n           }\r\n        }\r\n        \r\n        \r\n        return result;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('4e65b09f-0d5e-44a4-8f20-7cc5e85dce68', 'SevenTinyTest.UserInformation.Add_Before', '新增-前', null, null, '0', '1', '0', '2020-06-29 05:07:48', '0', '2020-06-29 05:07:48', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '1', '1', '1', 'MetaObjectInterfaceTrigger', 'Add_Before', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\nusing Chameleon.Repository;\r\nusing Newtonsoft.Json;\r\nusing Microsoft.Extensions.Logging;\r\n\r\npublic class MetaObjectInterfaceTrigger\r\n{\r\n    \r\n    //1. 记录日志\r\n    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();\r\n    /* \r\n        logger.LogDebug(string message, params object[] args);\r\n        logger.LogError(string message, params object[] args);\r\n        logger.LogInformation(string message, params object[] args);\r\n    */\r\n    //2. 查询数据\r\n    //MongoDb数据库查询上下文，需要时放开下行注释使用\r\n    //ChameleonDataDbContext dbContext = new ChameleonDataDbContext();\r\n    /* \r\n     *  查询数据的模板\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"key\",\"value\"),bf.Eq(\"key2\",\"value2\"));\r\n        dbContext.GetCollectionBson(\"对象编码\").Find(filter);\r\n    */\r\n\r\n    public BsonDocument Add_Before(Dictionary<string, object> triggerContext, BsonDocument bsonDocument)\r\n    {\r\n        return bsonDocument;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('4ecf6bda-e7c0-4ed0-b833-7b3788c59cf4', 'SevenTinyTest.TestJson', 'TestJson', null, null, '0', '0', '0', '2020-06-26 21:01:17', '0', '2020-06-26 21:30:07', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '3', '0', '0', '-', '-', '{\r\n    \'Key\':\'Item\'\r\n}');
INSERT INTO `TriggerScript` VALUES ('80116fa1-0941-4b8f-b1c4-22adae3ff60a', 'SevenTinyTest.Test', '测试', null, null, '0', '1', '0', '2020-06-26 20:10:48', '0', '2020-06-26 20:20:04', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    public Result<object> Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n        return Result<object>.Success();\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('87af6209-5180-4fe1-bbf2-27c405dc39b9', 'SevenTinyTest.TestJson', 'TestJson', null, null, '0', '1', '0', '2020-06-26 20:51:33', '0', '2020-06-26 20:51:33', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '3', '0', '0', '-', '-', '{\r\n    \"Key\":\"item\"\r\n}');
INSERT INTO `TriggerScript` VALUES ('87d837aa-82c5-4b81-a475-f7204f9836d6', 'SevenTinyTest.Test2', 'Test2', null, null, '0', '1', '0', '2020-06-26 20:51:14', '0', '2020-06-26 20:51:14', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    public Result<object> Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n        return Result<object>.Success();\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('92c75aa2-5f36-48f0-a228-dace69425217', 'SevenTinyTest.UserInformation.QueryList_After', '查询集合-后', null, null, '0', '1', '0', '2020-06-27 12:25:18', '0', '2020-06-27 12:25:18', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '1', '14', '1', 'MetaObjectInterfaceTrigger', 'QueryList_After', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\nusing Chameleon.Repository;\r\n\r\n\r\npublic class MetaObjectInterfaceTrigger\r\n{\r\n    \r\n    //记录日志\r\n    Microsoft.Extensions.Logging.ILogger logger = new SevenTiny.Bantina.Logging.LogManager();\r\n    //MongoDb数据库查询上下文\r\n    ChameleonDataDbContext dbContext = new ChameleonDataDbContext();\r\n    /* \r\n     *  查询数据的模板\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"key\",\"value\"),bf.Eq(\"key2\",\"value2\"));\r\n        dbContext.GetCollectionBson(\"对象编码\").Find(filter);\r\n    */\r\n\r\n    public List<Dictionary<string, CloudData>> QueryList_After(Dictionary<string, object> triggerContext, List<Dictionary<string, CloudData>> result)\r\n    {\r\n        return result;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('aa2efce4-e294-43a6-b112-f70c3433c578', 'SevenTinyTest.TestScript', 'TestScript', null, null, '0', '1', '0', '2020-06-26 22:16:14', '0', '2020-06-26 22:58:21', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\nusing Chameleon.Repository;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    //记录日志\r\n    Microsoft.Extensions.Logging.ILogger logger = new SevenTiny.Bantina.Logging.LogManager();\r\n    //MongoDb数据库查询上下文\r\n    ChameleonDataDbContext dbContext = new ChameleonDataDbContext();\r\n    /* \r\n     *  查询数据的模板\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"key\",\"value\"),bf.Eq(\"key2\",\"value2\"));\r\n        dbContext.GetCollectionBson(\"对象编码\").Find(filter);\r\n    */\r\n\r\n    public Result<object> Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n      	var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"Age\",\"90\"),bf.Eq(\"key2\",\"value2\"));\r\n        var result = dbContext.GetCollectionBson(\"SevenTinyTest.UserInformation\").Find(bf.Eq(\"Age\",90)).ToList();\r\n        return Result<object>.Success(data:result);\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('b3a9b2eb-6135-4b6b-bcf0-616b64f6efb7', 'SevenTinyTest.Test', '测试', null, null, '0', '1', '0', '2020-06-26 20:50:10', '0', '2020-06-26 20:50:10', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    public Result<object> Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n        return Result<object>.Success();\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('c1bd6612-78fa-4264-975c-a358ca44a117', 'SevenTinyTest.UserInformation.QueryList_Before', '查询集合-前', null, null, '0', '1', '0', '2020-06-25 23:05:19', '0', '2020-06-25 23:23:10', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '1ba123b9-a2e7-4602-b101-d9cc15a3925c', '1', '13', '1', 'Test', 'GetA', 'using System;\r\npublic class Test\r\n{\r\n    public int GetA(int a)\r\n    {\r\n        return a;\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('ccd8ab6d-3b1e-43eb-af7d-364d70ddb74f', 'SevenTinyTest.Test', '测试', null, null, '0', '1', '0', '2020-06-26 20:48:32', '0', '2020-06-26 20:48:32', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    public Result<object> Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n        return Result<object>.Success();\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('e72ef629-20b0-4dce-af73-73270ad2a601', 'SevenTinyTest.TestScript', 'TestScript', null, null, '0', '1', '0', '2020-06-27 12:33:27', '0', '2020-06-27 12:36:29', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\nusing Chameleon.Repository;\r\nusing Newtonsoft.Json;\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    //记录日志\r\n    Microsoft.Extensions.Logging.ILogger logger = new SevenTiny.Bantina.Logging.LogManager();\r\n    //MongoDb数据库查询上下文，需要时放开下行注释使用\r\n    ChameleonDataDbContext dbContext = new ChameleonDataDbContext();\r\n    /* \r\n     *  查询数据的模板\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"key\",\"value\"),bf.Eq(\"key2\",\"value2\"));\r\n        dbContext.GetCollectionBson(\"对象编码\").Find(filter);\r\n    */\r\n\r\n    public object Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n      //logger.Error($\"arg={JsonConvert.SerializeObject(argumentsUpperKeyDic)}\");\r\n        var bf = Builders<BsonDocument>.Filter;\r\n        var filter = bf.And(bf.Eq(\"Age\",\"90\"),bf.Eq(\"key2\",\"value2\"));\r\n        var result = dbContext.GetCollectionBson(\"SevenTinyTest.UserInformation\").Find(bf.Eq(\"Age\",90)).ToList();\r\n        return result;\r\n    }\r\n}');
INSERT INTO `TriggerScript` VALUES ('e93ba449-d2db-471e-8953-263b4ce46949', 'SevenTinyTest.TestScript', '测试脚本', null, null, '0', '1', '0', '2020-06-26 21:01:32', '0', '2020-06-26 21:17:28', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '2', '0', '1', 'DynamicScriptDataSource', 'Get', '//请不要修改脚本模板中的默认类名和方法名\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing MongoDB.Driver;\r\nusing MongoDB.Bson;\r\nusing SevenTiny.Bantina;\r\nusing Chameleon.ValueObject;\r\n\r\n\r\npublic class DynamicScriptDataSource\r\n{\r\n    \r\n    public Result<object> Get(Dictionary<string, string> argumentsUpperKeyDic)\r\n    {\r\n        return Result<object>.Success(\"这是一个接口\",\"xxxxxxxxxxxxxxx什么？？？？？\");\r\n    }\r\n}\r\n');
INSERT INTO `TriggerScript` VALUES ('fd970cd6-bee6-4df3-b2a8-b0e1683433ac', 'SevenTinyTest.DefaultJson', '默认Json', null, null, '0', '1', '0', '2020-06-26 20:37:01', '0', '2020-06-26 20:37:06', '0', 'a9361a2b-fb42-4deb-87f3-35b31fdd3e20', '00000000-0000-0000-0000-000000000000', '3', '0', '0', '-', '-', '{\r\n    \"Key\":\"item\"\r\n}');

-- ----------------------------
-- Table structure for UserAccount
-- ----------------------------
DROP TABLE IF EXISTS `UserAccount`;
CREATE TABLE `UserAccount` (
  `Id` char(36) NOT NULL,
  `Code` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Group` varchar(255) DEFAULT NULL,
  `SortNumber` int NOT NULL,
  `IsDeleted` int NOT NULL,
  `CreateBy` int NOT NULL,
  `CreateTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ModifyBy` int NOT NULL,
  `ModifyTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP,
  `TenantId` int NOT NULL,
  `Phone` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Password` varchar(255) NOT NULL,
  `IsNeedToResetPassword` int NOT NULL,
  `Role` int NOT NULL,
  `Identity` int NOT NULL,
  `Organization` char(36) NOT NULL,
  PRIMARY KEY (`Id`,`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of UserAccount
-- ----------------------------
