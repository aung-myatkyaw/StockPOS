/*
SQLyog Community v13.1.6 (64 bit)
MySQL - 5.7.11-log : Database - posxwin
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`posxwin` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `posxwin`;

/*Table structure for table `bought` */

DROP TABLE IF EXISTS `bought`;

CREATE TABLE `bought` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Barcode` varchar(30) DEFAULT NULL,
  `StockAmount` int(11) DEFAULT NULL,
  `SupplierID` int(11) DEFAULT NULL,
  `BoughtDateTime` datetime DEFAULT NULL,
  `TotalAmount` double DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

/*Data for the table `bought` */

insert  into `bought`(`ID`,`Barcode`,`StockAmount`,`SupplierID`,`BoughtDateTime`,`TotalAmount`) values 
(1,'123456',23,1,'2021-10-19 21:31:21',46),
(2,'456123',2,3,'2021-10-12 21:31:23',82),
(4,'798985',3,12,'2021-10-11 02:29:29',12);

/*Table structure for table `cashier` */

DROP TABLE IF EXISTS `cashier`;

CREATE TABLE `cashier` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(50) DEFAULT NULL,
  `UserName` varchar(50) NOT NULL,
  `DateofBirth` datetime DEFAULT NULL,
  `Gender` varchar(10) DEFAULT NULL,
  `Address` varchar(40) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `Email` varchar(30) DEFAULT NULL,
  `Password` varchar(255) NOT NULL,
  `PasswordSalt` varchar(255) NOT NULL,
  `DateCreated` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

/*Data for the table `cashier` */

insert  into `cashier`(`ID`,`FullName`,`UserName`,`DateofBirth`,`Gender`,`Address`,`Phone`,`Email`,`Password`,`PasswordSalt`,`DateCreated`) values 
(1,'Josh','josh','2000-10-18 21:49:24','Male','Ygn','09777245486','josh@bush.to','3tsIddTOFo9bTF5mUeRJwxPIYFMzbtce','e8crscWSC7SM/ETN0/LVwQaqBsSVv3Gz','2021-10-18 18:38:27'),
(2,'Josh','abc','2000-10-18 21:49:24','Male','Ygn','09777245486','josh@bush.to','zJUJwsoEjrPW3bSNYoULCV/+3xlv1v7C','QKWBhK4cK76CHCjxVl5V1kqm3hy//8k9','2021-10-18 19:50:14'),
(3,'Martha','martha','2000-10-18 21:49:24','Female','Ygn','09777245486','martha@bush.to','lCpj+aOTdJMLx4Q/eHdlxEqW9RIE3yLP','698X4moS4a8iXHzOtuZni9E4tw+dbghA','2021-10-19 15:09:10');

/*Table structure for table `eventlog` */

DROP TABLE IF EXISTS `eventlog`;

CREATE TABLE `eventlog` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `LogType` int(11) DEFAULT NULL COMMENT 'Info = 1, Error = 2,Warning = 3, Insert = 4,Update = 5, Delete = 6',
  `LogDateTime` datetime DEFAULT NULL,
  `Source` varchar(100) DEFAULT NULL,
  `FormName` varchar(100) DEFAULT NULL,
  `LogMessage` text,
  `ErrorMessage` text,
  `UserID` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `user_id` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=65 DEFAULT CHARSET=utf8;

/*Data for the table `eventlog` */

insert  into `eventlog`(`ID`,`LogType`,`LogDateTime`,`Source`,`FormName`,`LogMessage`,`ErrorMessage`,`UserID`) values 
(1,5,'2021-10-18 10:04:04','api','BoughtController.PutBought','Updated :\r\nTranId : 3\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),
(2,4,'2021-10-18 10:19:59','api','BoughtController.PostBought','Created :\r\nTranId : 4\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 100\r\n','',NULL),
(3,5,'2021-10-18 10:20:16','api','BoughtController.PutBought','Updated :\r\nTranId : 3\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),
(4,5,'2021-10-18 10:20:30','api','BoughtController.PutBought','Updated :\r\nTranId : 4\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),
(5,6,'2021-10-18 10:21:02','api','BoughtController.DeleteBought','Deleted :\r\nTranId : 3\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),
(6,4,'2021-10-18 18:38:28','api','CashierController.RegisterCashier','Created :\r\nId : 3\r\nFullName : Josh\r\nUserName : josh\r\nDateofBirth : 18-Oct-00 9:49:24 PM\r\nGender : Male\r\nAddress : Ygn\r\nPhone : 09777245486\r\nEmail : josh@bush.to\r\nPassword : 3tsIddTOFo9bTF5mUeRJwxPIYFMzbtce\r\nPasswordSalt : e8crscWSC7SM/ETN0/LVwQaqBsSVv3Gz\r\nDateCreated : 18-Oct-21 6:38:27 PM\r\n','',NULL),
(7,2,'2021-10-18 19:40:40','api','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(8,2,'2021-10-18 19:40:46','api','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(9,2,'2021-10-18 19:44:34','api','','Generate Token Fail forjosh','\'MultiPOS.Models.Cashier\' does not contain a definition for \'Name\'',NULL),
(10,1,'2021-10-18 19:46:47','api','','Successful login for this account UserName: josh','',1),
(11,3,'2021-10-18 19:47:40','api','','Token not found','',NULL),
(12,4,'2021-10-18 19:50:14','api','CashierController.RegisterCashier','Created :\r\nId : 2\r\nFullName : Josh\r\nUserName : abc\r\nDateofBirth : 18-Oct-00 9:49:24 PM\r\nGender : Male\r\nAddress : Ygn\r\nPhone : 09777245486\r\nEmail : josh@bush.to\r\nPassword : zJUJwsoEjrPW3bSNYoULCV/+3xlv1v7C\r\nPasswordSalt : QKWBhK4cK76CHCjxVl5V1kqm3hy//8k9\r\nDateCreated : 18-Oct-21 7:50:13 PM\r\n','',NULL),
(13,1,'2021-10-18 19:50:39','api','','Successful login for this account UserName: abc','',2),
(14,3,'2021-10-18 19:51:41','','','Token not found','',NULL),
(15,3,'2021-10-18 19:52:24','health','','Token not found','',NULL),
(16,3,'2021-10-18 19:52:34','health','','Token not found','',NULL),
(17,2,'2021-10-19 14:06:42','api','','Invalid login credentials','UserName:, Password: 1234',NULL),
(18,2,'2021-10-19 14:12:10','/api/token','','Invalid login credentials','UserName:, Password: 1234',NULL),
(19,1,'2021-10-19 14:13:18','/api/token','','Successful login for this account UserName: josh','',1),
(20,2,'2021-10-19 14:17:45','/api/token','','User not found with ab','',NULL),
(21,3,'2021-10-19 14:38:24','/api/cashier','','The Token has expired','',NULL),
(22,3,'2021-10-19 14:38:32','/api/token','','Cashier not found with UserName: ab','',NULL),
(23,1,'2021-10-19 14:38:44','/api/token','','Successful login for this account UserName: josh','',1),
(24,3,'2021-10-19 14:39:03','/api/cashier','','The Token has expired','',NULL),
(25,3,'2021-10-19 14:39:11','/api/cashier','','The Token has expired','',NULL),
(26,3,'2021-10-19 14:39:24','/api/cashier','','The Token has expired','',NULL),
(27,1,'2021-10-19 14:39:36','/api/token','','Successful login for this account UserName: josh','',1),
(28,3,'2021-10-19 14:39:49','/api/cashier','','The Token has expired','',NULL),
(29,1,'2021-10-19 14:59:40','/api/token','','Successful login for this account UserName: josh','',1),
(30,3,'2021-10-19 14:59:40','/api/token','','Token not found','',1),
(31,3,'2021-10-19 15:00:54','/api/bought','','Token not found','',NULL),
(32,3,'2021-10-19 15:05:55','/api/bought','','Token not found','',NULL),
(33,4,'2021-10-19 15:06:02','/api/bought','BoughtController.PostBought','Created :\r\nId : 5\r\nBarcode : 894315\r\nStockAmount : 60\r\nSupplierId : 1\r\nBoughtDateTime : 19-Oct-21 9:31:21 PM\r\nTotalAmount : 46\r\n','',1),
(34,3,'2021-10-19 15:06:45','/api/bought/4','','Token not found','',NULL),
(35,5,'2021-10-19 15:07:25','/api/bought/5','BoughtController.PutBought','Updated :\r\nId : 5\r\nBarcode : 894315\r\nStockAmount : 60\r\nSupplierId : 1\r\nBoughtDateTime : 19-Oct-21 9:31:21 PM\r\nTotalAmount : 60\r\n','',1),
(36,3,'2021-10-19 15:07:39','/api/bought/5','','Token not found','',NULL),
(37,6,'2021-10-19 15:07:45','/api/bought/5','BoughtController.DeleteBought','Deleted :\r\nId : 5\r\nBarcode : 894315\r\nStockAmount : 60\r\nSupplierId : 1\r\nBoughtDateTime : 19-Oct-21 9:31:21 PM\r\nTotalAmount : 60\r\n','',1),
(38,4,'2021-10-19 15:09:10','/api/cashier/Registration','CashierController.RegisterCashier','Created :\r\nId : 3\r\nFullName : Martha\r\nUserName : martha\r\nDateofBirth : 18-Oct-00 9:49:24 PM\r\nGender : Female\r\nAddress : Ygn\r\nPhone : 09777245486\r\nEmail : martha@bush.to\r\nPassword : lCpj+aOTdJMLx4Q/eHdlxEqW9RIE3yLP\r\nPasswordSalt : 698X4moS4a8iXHzOtuZni9E4tw+dbghA\r\nDateCreated : 19-Oct-21 3:09:10 PM\r\n','',NULL),
(39,1,'2021-10-19 15:09:29','/api/token','','Successful login for this account UserName: martha','',3),
(40,3,'2021-10-19 15:09:29','/api/token','','Token not found','',3),
(41,1,'2021-10-19 15:10:17','/api/token','','Successful login for this account UserName: martha','',3),
(42,3,'2021-10-19 15:10:17','/api/token','','Token not found','',3),
(43,1,'2021-10-19 15:10:50','/api/token','','Successful login for this account UserName: martha','',3),
(44,3,'2021-10-19 15:10:50','/api/token','','Token not found','',3),
(45,1,'2021-10-19 15:12:32','/api/token','','Successful login for this account UserName: martha','',3),
(46,1,'2021-10-19 15:13:16','/api/token','','Successful login for this account UserName: martha','',3),
(47,4,'2021-10-19 15:15:07','/api/sale','SaleController.PostSale','Created :\r\nId : 2\r\nVouncherNum : 456789\r\nCashierId : 1\r\nSaleDateTime : 19-Oct-21 9:33:08 PM\r\nProductBarcode : 123456\r\nQuantity : 10\r\nTotalAmount : 15000\r\n','',1),
(48,5,'2021-10-19 15:16:48','/api/sale/2','SaleController.PutSale','Updated :\r\nId : 2\r\nVouncherNum : 456789\r\nCashierId : 1\r\nSaleDateTime : 19-Oct-21 9:33:08 PM\r\nProductBarcode : 123456\r\nQuantity : 20\r\nTotalAmount : 15000\r\n','',1),
(49,6,'2021-10-19 15:17:03','/api/sale/2','SaleController.DeleteSale','Deleted :\r\nId : 2\r\nVouncherNum : 456789\r\nCashierId : 1\r\nSaleDateTime : 19-Oct-21 9:33:08 PM\r\nProductBarcode : 123456\r\nQuantity : 20\r\nTotalAmount : 15000\r\n','',1),
(50,4,'2021-10-19 15:19:08','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(51,2,'2021-10-19 15:19:08','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),
(52,4,'2021-10-19 15:20:15','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(53,2,'2021-10-19 15:20:15','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),
(54,4,'2021-10-19 15:21:19','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(55,2,'2021-10-19 15:21:49','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),
(56,4,'2021-10-19 15:22:40','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(57,2,'2021-10-19 15:24:08','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),
(58,4,'2021-10-19 15:24:50','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(59,4,'2021-10-19 15:26:29','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(60,5,'2021-10-19 15:27:48','/api/product/456789','ProductController.PutProduct','Updated :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 730\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(61,6,'2021-10-19 15:28:20','/api/product/456789','ProductController.DeleteProduct','Deleted :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 730\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),
(62,4,'2021-10-19 15:29:38','/api/productcategory','ProductcategoryController.PostProductcategory','Created :\r\nId : 2\r\nName : Category 2\r\nDateCreated : 19-Oct-21 9:32:52 PM\r\n','',1),
(63,5,'2021-10-19 15:30:13','/api/productcategory/2','ProductcategoryController.PutProductcategory','Updated :\r\nId : 2\r\nName : Testing 2\r\nDateCreated : 19-Oct-21 9:32:52 PM\r\n','',1),
(64,6,'2021-10-19 15:30:25','/api/productcategory/2','ProductcategoryController.DeleteProductcategory','Deleted :\r\nId : 2\r\nName : Testing 2\r\nDateCreated : 19-Oct-21 9:32:52 PM\r\n','',1);

/*Table structure for table `product` */

DROP TABLE IF EXISTS `product`;

CREATE TABLE `product` (
  `Barcode` varchar(30) NOT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `CategoryID` int(11) DEFAULT NULL,
  `BoughtPrice` double DEFAULT NULL,
  `SellPriceRetail` double DEFAULT NULL,
  `SellPricewhole` double DEFAULT NULL,
  `DateAdded` datetime DEFAULT NULL,
  `AlertQuantity` int(11) DEFAULT NULL,
  PRIMARY KEY (`Barcode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `product` */

insert  into `product`(`Barcode`,`Name`,`CategoryID`,`BoughtPrice`,`SellPriceRetail`,`SellPricewhole`,`DateAdded`,`AlertQuantity`) values 
('123456','Test',1,200,450,455,'2021-10-19 21:32:33',2);

/*Table structure for table `productcategory` */

DROP TABLE IF EXISTS `productcategory`;

CREATE TABLE `productcategory` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

/*Data for the table `productcategory` */

insert  into `productcategory`(`ID`,`Name`,`DateCreated`) values 
(1,'Testing','2021-10-19 21:32:52');

/*Table structure for table `sale` */

DROP TABLE IF EXISTS `sale`;

CREATE TABLE `sale` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `VouncherNum` varchar(30) DEFAULT NULL,
  `CashierID` int(11) DEFAULT NULL,
  `SaleDateTime` datetime DEFAULT NULL,
  `ProductBarcode` varchar(30) DEFAULT NULL,
  `Quantity` int(11) DEFAULT NULL,
  `TotalAmount` double DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

/*Data for the table `sale` */

insert  into `sale`(`ID`,`VouncherNum`,`CashierID`,`SaleDateTime`,`ProductBarcode`,`Quantity`,`TotalAmount`) values 
(1,'123456',1,'2021-10-19 21:33:08','123456',5,10000);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
