/*
Navicat MySQL Data Transfer

Source Server         : 81.68.136.47
Source Server Version : 80020
Source Host           : 81.68.136.47:39901
Source Database       : SevenTinyConfig

Target Server Type    : MYSQL
Target Server Version : 80020
File Encoding         : 65001

Date: 2020-06-28 21:24:33
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for ConnectionStrings
-- ----------------------------
DROP TABLE IF EXISTS `ConnectionStrings`;
CREATE TABLE `ConnectionStrings` (
  `mongodb39911` varchar(1000) NOT NULL,
  `MultiTenantPlatformWeb` varchar(1000) NOT NULL,
  `mysql39901` varchar(1000) NOT NULL,
  `MultiTenantAccount` varchar(1000) NOT NULL,
  `Chameleon` varchar(1000) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for FaaS_CSharpAssemblyReference
-- ----------------------------
DROP TABLE IF EXISTS `FaaS_CSharpAssemblyReference`;
CREATE TABLE `FaaS_CSharpAssemblyReference` (
  `AppName` varchar(255) NOT NULL,
  `Assembly` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for FaaS_Settings
-- ----------------------------
DROP TABLE IF EXISTS `FaaS_Settings`;
CREATE TABLE `FaaS_Settings` (
  `IsDebugMode` int NOT NULL COMMENT '是否Debug编译',
  `IsOutPutFiles` int NOT NULL,
  `ReferenceDirs` varchar(255) DEFAULT NULL COMMENT '要引用的dll的路径'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
