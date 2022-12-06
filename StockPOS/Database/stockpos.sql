-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: localhost    Database: stockpos
-- ------------------------------------------------------
-- Server version	8.0.31

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE DATABASE /*!32312 IF NOT EXISTS*/`stockpos` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `stockpos`;

--
-- Table structure for table `changedpricelog`
--

DROP TABLE IF EXISTS `changedpricelog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `changedpricelog` (
  `PriceLogID` int NOT NULL AUTO_INCREMENT,
  `ProductID` varchar(15) DEFAULT NULL,
  `ChangedDate` datetime DEFAULT NULL,
  `LatestBoughtPrice` double DEFAULT NULL,
  `LatestRetailPrice` double DEFAULT NULL,
  `LatestWholePrice` double DEFAULT NULL,
  PRIMARY KEY (`PriceLogID`),
  KEY `PriceLogToProduct_idx` (`ProductID`),
  CONSTRAINT `PriceLogToProduct` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `changedpricelog`
--

LOCK TABLES `changedpricelog` WRITE;
/*!40000 ALTER TABLE `changedpricelog` DISABLE KEYS */;
/*!40000 ALTER TABLE `changedpricelog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customer`
--

DROP TABLE IF EXISTS `customer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customer` (
  `CustomerID` int NOT NULL AUTO_INCREMENT,
  `CustomerName` varchar(45) DEFAULT NULL,
  `VillageID` int DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `UpdatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`CustomerID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customer`
--

LOCK TABLES `customer` WRITE;
/*!40000 ALTER TABLE `customer` DISABLE KEYS */;
/*!40000 ALTER TABLE `customer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debtbalance`
--

DROP TABLE IF EXISTS `debtbalance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debtbalance` (
  `DebtBalanceID` int NOT NULL AUTO_INCREMENT,
  `CustomerID` int DEFAULT NULL,
  `DebtAmount` double DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `UserID` int DEFAULT NULL,
  `SaleID` int DEFAULT NULL,
  PRIMARY KEY (`DebtBalanceID`),
  KEY `DebtToCustomer_idx` (`CustomerID`),
  KEY `DebtToUserID_idx` (`UserID`),
  KEY `DebtToSale_idx` (`SaleID`),
  CONSTRAINT `DebtToCustomer` FOREIGN KEY (`CustomerID`) REFERENCES `customer` (`CustomerID`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `DebtToSale` FOREIGN KEY (`SaleID`) REFERENCES `sale` (`SaleID`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `DebtToUserID` FOREIGN KEY (`UserID`) REFERENCES `user` (`UserID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debtbalance`
--

LOCK TABLES `debtbalance` WRITE;
/*!40000 ALTER TABLE `debtbalance` DISABLE KEYS */;
/*!40000 ALTER TABLE `debtbalance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventlog`
--

DROP TABLE IF EXISTS `eventlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `eventlog` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `LogType` int DEFAULT NULL COMMENT 'Info = 1, Error = 2,Warning = 3, Insert = 4,Update = 5, Delete = 6',
  `LogDateTime` datetime DEFAULT NULL,
  `Source` varchar(100) DEFAULT NULL,
  `FormName` varchar(100) DEFAULT NULL,
  `LogMessage` text,
  `ErrorMessage` text,
  `UserID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `user_id` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=66 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventlog`
--

LOCK TABLES `eventlog` WRITE;
/*!40000 ALTER TABLE `eventlog` DISABLE KEYS */;
INSERT INTO `eventlog` VALUES (1,5,'2021-10-18 10:04:04','api','BoughtController.PutBought','Updated :\r\nTranId : 3\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),(2,4,'2021-10-18 10:19:59','api','BoughtController.PostBought','Created :\r\nTranId : 4\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 100\r\n','',NULL),(3,5,'2021-10-18 10:20:16','api','BoughtController.PutBought','Updated :\r\nTranId : 3\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),(4,5,'2021-10-18 10:20:30','api','BoughtController.PutBought','Updated :\r\nTranId : 4\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),(5,6,'2021-10-18 10:21:02','api','BoughtController.DeleteBought','Deleted :\r\nTranId : 3\r\nBarcode : 798985\r\nStockAmount : 3\r\nSupplierId : 12\r\nDateAdded : \r\nTotalAmount : 82\r\n','',NULL),(6,4,'2021-10-18 18:38:28','api','CashierController.RegisterCashier','Created :\r\nId : 3\r\nFullName : Josh\r\nUserName : josh\r\nDateofBirth : 18-Oct-00 9:49:24 PM\r\nGender : Male\r\nAddress : Ygn\r\nPhone : 09777245486\r\nEmail : josh@bush.to\r\nPassword : 3tsIddTOFo9bTF5mUeRJwxPIYFMzbtce\r\nPasswordSalt : e8crscWSC7SM/ETN0/LVwQaqBsSVv3Gz\r\nDateCreated : 18-Oct-21 6:38:27 PM\r\n','',NULL),(7,2,'2021-10-18 19:40:40','api','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),(8,2,'2021-10-18 19:40:46','api','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),(9,2,'2021-10-18 19:44:34','api','','Generate Token Fail forjosh','\'MultiPOS.Models.Cashier\' does not contain a definition for \'Name\'',NULL),(10,1,'2021-10-18 19:46:47','api','','Successful login for this account UserName: josh','',1),(11,3,'2021-10-18 19:47:40','api','','Token not found','',NULL),(12,4,'2021-10-18 19:50:14','api','CashierController.RegisterCashier','Created :\r\nId : 2\r\nFullName : Josh\r\nUserName : abc\r\nDateofBirth : 18-Oct-00 9:49:24 PM\r\nGender : Male\r\nAddress : Ygn\r\nPhone : 09777245486\r\nEmail : josh@bush.to\r\nPassword : zJUJwsoEjrPW3bSNYoULCV/+3xlv1v7C\r\nPasswordSalt : QKWBhK4cK76CHCjxVl5V1kqm3hy//8k9\r\nDateCreated : 18-Oct-21 7:50:13 PM\r\n','',NULL),(13,1,'2021-10-18 19:50:39','api','','Successful login for this account UserName: abc','',2),(14,3,'2021-10-18 19:51:41','','','Token not found','',NULL),(15,3,'2021-10-18 19:52:24','health','','Token not found','',NULL),(16,3,'2021-10-18 19:52:34','health','','Token not found','',NULL),(17,2,'2021-10-19 14:06:42','api','','Invalid login credentials','UserName:, Password: 1234',NULL),(18,2,'2021-10-19 14:12:10','/api/token','','Invalid login credentials','UserName:, Password: 1234',NULL),(19,1,'2021-10-19 14:13:18','/api/token','','Successful login for this account UserName: josh','',1),(20,2,'2021-10-19 14:17:45','/api/token','','User not found with ab','',NULL),(21,3,'2021-10-19 14:38:24','/api/cashier','','The Token has expired','',NULL),(22,3,'2021-10-19 14:38:32','/api/token','','Cashier not found with UserName: ab','',NULL),(23,1,'2021-10-19 14:38:44','/api/token','','Successful login for this account UserName: josh','',1),(24,3,'2021-10-19 14:39:03','/api/cashier','','The Token has expired','',NULL),(25,3,'2021-10-19 14:39:11','/api/cashier','','The Token has expired','',NULL),(26,3,'2021-10-19 14:39:24','/api/cashier','','The Token has expired','',NULL),(27,1,'2021-10-19 14:39:36','/api/token','','Successful login for this account UserName: josh','',1),(28,3,'2021-10-19 14:39:49','/api/cashier','','The Token has expired','',NULL),(29,1,'2021-10-19 14:59:40','/api/token','','Successful login for this account UserName: josh','',1),(30,3,'2021-10-19 14:59:40','/api/token','','Token not found','',1),(31,3,'2021-10-19 15:00:54','/api/bought','','Token not found','',NULL),(32,3,'2021-10-19 15:05:55','/api/bought','','Token not found','',NULL),(33,4,'2021-10-19 15:06:02','/api/bought','BoughtController.PostBought','Created :\r\nId : 5\r\nBarcode : 894315\r\nStockAmount : 60\r\nSupplierId : 1\r\nBoughtDateTime : 19-Oct-21 9:31:21 PM\r\nTotalAmount : 46\r\n','',1),(34,3,'2021-10-19 15:06:45','/api/bought/4','','Token not found','',NULL),(35,5,'2021-10-19 15:07:25','/api/bought/5','BoughtController.PutBought','Updated :\r\nId : 5\r\nBarcode : 894315\r\nStockAmount : 60\r\nSupplierId : 1\r\nBoughtDateTime : 19-Oct-21 9:31:21 PM\r\nTotalAmount : 60\r\n','',1),(36,3,'2021-10-19 15:07:39','/api/bought/5','','Token not found','',NULL),(37,6,'2021-10-19 15:07:45','/api/bought/5','BoughtController.DeleteBought','Deleted :\r\nId : 5\r\nBarcode : 894315\r\nStockAmount : 60\r\nSupplierId : 1\r\nBoughtDateTime : 19-Oct-21 9:31:21 PM\r\nTotalAmount : 60\r\n','',1),(38,4,'2021-10-19 15:09:10','/api/cashier/Registration','CashierController.RegisterCashier','Created :\r\nId : 3\r\nFullName : Martha\r\nUserName : martha\r\nDateofBirth : 18-Oct-00 9:49:24 PM\r\nGender : Female\r\nAddress : Ygn\r\nPhone : 09777245486\r\nEmail : martha@bush.to\r\nPassword : lCpj+aOTdJMLx4Q/eHdlxEqW9RIE3yLP\r\nPasswordSalt : 698X4moS4a8iXHzOtuZni9E4tw+dbghA\r\nDateCreated : 19-Oct-21 3:09:10 PM\r\n','',NULL),(39,1,'2021-10-19 15:09:29','/api/token','','Successful login for this account UserName: martha','',3),(40,3,'2021-10-19 15:09:29','/api/token','','Token not found','',3),(41,1,'2021-10-19 15:10:17','/api/token','','Successful login for this account UserName: martha','',3),(42,3,'2021-10-19 15:10:17','/api/token','','Token not found','',3),(43,1,'2021-10-19 15:10:50','/api/token','','Successful login for this account UserName: martha','',3),(44,3,'2021-10-19 15:10:50','/api/token','','Token not found','',3),(45,1,'2021-10-19 15:12:32','/api/token','','Successful login for this account UserName: martha','',3),(46,1,'2021-10-19 15:13:16','/api/token','','Successful login for this account UserName: martha','',3),(47,4,'2021-10-19 15:15:07','/api/sale','SaleController.PostSale','Created :\r\nId : 2\r\nVouncherNum : 456789\r\nCashierId : 1\r\nSaleDateTime : 19-Oct-21 9:33:08 PM\r\nProductBarcode : 123456\r\nQuantity : 10\r\nTotalAmount : 15000\r\n','',1),(48,5,'2021-10-19 15:16:48','/api/sale/2','SaleController.PutSale','Updated :\r\nId : 2\r\nVouncherNum : 456789\r\nCashierId : 1\r\nSaleDateTime : 19-Oct-21 9:33:08 PM\r\nProductBarcode : 123456\r\nQuantity : 20\r\nTotalAmount : 15000\r\n','',1),(49,6,'2021-10-19 15:17:03','/api/sale/2','SaleController.DeleteSale','Deleted :\r\nId : 2\r\nVouncherNum : 456789\r\nCashierId : 1\r\nSaleDateTime : 19-Oct-21 9:33:08 PM\r\nProductBarcode : 123456\r\nQuantity : 20\r\nTotalAmount : 15000\r\n','',1),(50,4,'2021-10-19 15:19:08','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(51,2,'2021-10-19 15:19:08','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),(52,4,'2021-10-19 15:20:15','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(53,2,'2021-10-19 15:20:15','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),(54,4,'2021-10-19 15:21:19','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(55,2,'2021-10-19 15:21:49','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),(56,4,'2021-10-19 15:22:40','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(57,2,'2021-10-19 15:24:08','/api/product','ProductController.PostProduct','New Token Generation Failed','No route matches the supplied values.',1),(58,4,'2021-10-19 15:24:50','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(59,4,'2021-10-19 15:26:29','/api/product','ProductController.PostProduct','Created :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 700\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(60,5,'2021-10-19 15:27:48','/api/product/456789','ProductController.PutProduct','Updated :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 730\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(61,6,'2021-10-19 15:28:20','/api/product/456789','ProductController.DeleteProduct','Deleted :\r\nBarcode : 456789\r\nName : Test2\r\nCategoryId : 1\r\nBoughtPrice : 500\r\nSellPriceRetail : 730\r\nSellPricewhole : 750\r\nDateAdded : 19-Oct-21 9:32:33 PM\r\nAlertQuantity : 1\r\n','',1),(62,4,'2021-10-19 15:29:38','/api/productcategory','ProductcategoryController.PostProductcategory','Created :\r\nId : 2\r\nName : Category 2\r\nDateCreated : 19-Oct-21 9:32:52 PM\r\n','',1),(63,5,'2021-10-19 15:30:13','/api/productcategory/2','ProductcategoryController.PutProductcategory','Updated :\r\nId : 2\r\nName : Testing 2\r\nDateCreated : 19-Oct-21 9:32:52 PM\r\n','',1),(64,6,'2021-10-19 15:30:25','/api/productcategory/2','ProductcategoryController.DeleteProductcategory','Deleted :\r\nId : 2\r\nName : Testing 2\r\nDateCreated : 19-Oct-21 9:32:52 PM\r\n','',1),(65,1,'2022-11-30 16:27:50','/api/token','','Successful login for this account UserName: martha','',3);
/*!40000 ALTER TABLE `eventlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupvillages`
--

DROP TABLE IF EXISTS `groupvillages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groupvillages` (
  `GroupVillageID` int NOT NULL AUTO_INCREMENT,
  `GroupVillageName` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `GroupVillageShortName` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `OtherColumn1` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`GroupVillageID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupvillages`
--

LOCK TABLES `groupvillages` WRITE;
/*!40000 ALTER TABLE `groupvillages` DISABLE KEYS */;
/*!40000 ALTER TABLE `groupvillages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `ProductID` varchar(15) NOT NULL,
  `Barcode` varchar(30) DEFAULT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `ShortName` varchar(10) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `CategoryID` varchar(15) DEFAULT NULL,
  `ProductTypeID` varchar(15) DEFAULT NULL,
  `BrandID` varchar(15) DEFAULT NULL,
  `BoughtPrice` double DEFAULT NULL,
  `SellPriceRetail` double DEFAULT NULL,
  `SellPricewhole` double DEFAULT NULL,
  `UpdatedDate` datetime DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ImageUrl` varchar(200) DEFAULT NULL,
  `AlertQuantity` int DEFAULT NULL,
  `IsVisible` tinyint DEFAULT NULL,
  PRIMARY KEY (`ProductID`),
  UNIQUE KEY `Barcode_UNIQUE` (`Barcode`),
  KEY `CategoryTable_idx` (`CategoryID`),
  KEY `ProductType_idx` (`ProductTypeID`),
  KEY `ProductBrand_idx` (`BrandID`),
  CONSTRAINT `CategoryTable` FOREIGN KEY (`CategoryID`) REFERENCES `productcategory` (`CategoryID`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `ProductBrand` FOREIGN KEY (`BrandID`) REFERENCES `productbrand` (`BrandID`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `ProductType` FOREIGN KEY (`ProductTypeID`) REFERENCES `producttype` (`ProductTypeID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES ('P000001','123456','Test',NULL,'1',NULL,NULL,200,450,455,NULL,'2021-10-19 21:32:33',NULL,2,NULL);
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productbrand`
--

DROP TABLE IF EXISTS `productbrand`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productbrand` (
  `BrandID` varchar(15) NOT NULL,
  `BrandName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`BrandID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productbrand`
--

LOCK TABLES `productbrand` WRITE;
/*!40000 ALTER TABLE `productbrand` DISABLE KEYS */;
/*!40000 ALTER TABLE `productbrand` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productcategory`
--

DROP TABLE IF EXISTS `productcategory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productcategory` (
  `CategoryID` varchar(15) NOT NULL,
  `CategoryName` varchar(100) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Visible` tinyint DEFAULT NULL,
  PRIMARY KEY (`CategoryID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productcategory`
--

LOCK TABLES `productcategory` WRITE;
/*!40000 ALTER TABLE `productcategory` DISABLE KEYS */;
INSERT INTO `productcategory` VALUES ('1','Testing','2021-10-19 21:32:52',NULL);
/*!40000 ALTER TABLE `productcategory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productsize`
--

DROP TABLE IF EXISTS `productsize`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productsize` (
  `ProductSizeID` int NOT NULL AUTO_INCREMENT,
  `ProductSizeName` varchar(45) DEFAULT NULL,
  `ProductTypeID` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`ProductSizeID`),
  KEY `ProuctTypeTable_idx` (`ProductTypeID`),
  CONSTRAINT `ProuctTypeTable` FOREIGN KEY (`ProductTypeID`) REFERENCES `producttype` (`ProductTypeID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productsize`
--

LOCK TABLES `productsize` WRITE;
/*!40000 ALTER TABLE `productsize` DISABLE KEYS */;
/*!40000 ALTER TABLE `productsize` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `producttype`
--

DROP TABLE IF EXISTS `producttype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `producttype` (
  `ProductTypeID` varchar(15) NOT NULL,
  `ProductTypeName` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ProductTypeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producttype`
--

LOCK TABLES `producttype` WRITE;
/*!40000 ALTER TABLE `producttype` DISABLE KEYS */;
/*!40000 ALTER TABLE `producttype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sale`
--

DROP TABLE IF EXISTS `sale`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sale` (
  `SaleID` int NOT NULL AUTO_INCREMENT,
  `VouncherNum` varchar(30) DEFAULT NULL,
  `UserID` int DEFAULT NULL,
  `SaleDateTime` datetime DEFAULT NULL,
  `ProductID` varchar(15) DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `TotalAmount` double DEFAULT NULL,
  `ProductPrice` double DEFAULT NULL,
  PRIMARY KEY (`SaleID`),
  KEY `SaleToUser_idx` (`UserID`),
  KEY `SaleToProduct_idx` (`ProductID`),
  CONSTRAINT `SaleToProduct` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `SaleToUser` FOREIGN KEY (`UserID`) REFERENCES `user` (`UserID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sale`
--

LOCK TABLES `sale` WRITE;
/*!40000 ALTER TABLE `sale` DISABLE KEYS */;
INSERT INTO `sale` VALUES (1,'123456',1,'2021-10-19 21:33:08','P000001',5,10000,NULL);
/*!40000 ALTER TABLE `sale` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `searchedcount`
--

DROP TABLE IF EXISTS `searchedcount`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `searchedcount` (
  `SearchID` int NOT NULL AUTO_INCREMENT,
  `Count` int DEFAULT NULL,
  `ProductID` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`SearchID`),
  KEY `SearchToProduct_idx` (`ProductID`),
  CONSTRAINT `SearchToProduct` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `searchedcount`
--

LOCK TABLES `searchedcount` WRITE;
/*!40000 ALTER TABLE `searchedcount` DISABLE KEYS */;
/*!40000 ALTER TABLE `searchedcount` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `UserID` int NOT NULL AUTO_INCREMENT,
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
  `UserTypeID` int NOT NULL,
  PRIMARY KEY (`UserID`),
  KEY `UserToUserType_idx` (`UserTypeID`),
  CONSTRAINT `UserToUserType` FOREIGN KEY (`UserTypeID`) REFERENCES `usertype` (`UserTypeID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'Josh','josh','2000-10-18 21:49:24','Male','Ygn','09777245486','josh@bush.to','3tsIddTOFo9bTF5mUeRJwxPIYFMzbtce','e8crscWSC7SM/ETN0/LVwQaqBsSVv3Gz','2021-10-18 18:38:27',1),(2,'Josh','abc','2000-10-18 21:49:24','Male','Ygn','09777245486','josh@bush.to','zJUJwsoEjrPW3bSNYoULCV/+3xlv1v7C','QKWBhK4cK76CHCjxVl5V1kqm3hy//8k9','2021-10-18 19:50:14',1),(3,'Martha','martha','2000-10-18 21:49:24','Female','Ygn','09777245486','martha@bush.to','lCpj+aOTdJMLx4Q/eHdlxEqW9RIE3yLP','698X4moS4a8iXHzOtuZni9E4tw+dbghA','2021-10-19 15:09:10',1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usertype`
--

DROP TABLE IF EXISTS `usertype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usertype` (
  `UserTypeID` int NOT NULL AUTO_INCREMENT,
  `UserTypeName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`UserTypeID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usertype`
--

LOCK TABLES `usertype` WRITE;
/*!40000 ALTER TABLE `usertype` DISABLE KEYS */;
INSERT INTO `usertype` VALUES (1,'Cashier'),(2,'Admin'),(3,'Manager');
/*!40000 ALTER TABLE `usertype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `villages`
--

DROP TABLE IF EXISTS `villages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `villages` (
  `VillageID` int NOT NULL AUTO_INCREMENT,
  `VillageName` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `VillageShortName` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `GroupVillageID` int DEFAULT NULL,
  PRIMARY KEY (`VillageID`),
  KEY `VillageToGroupVillage_idx` (`GroupVillageID`),
  CONSTRAINT `VillageToGroupVillage` FOREIGN KEY (`GroupVillageID`) REFERENCES `groupvillages` (`GroupVillageID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `villages`
--

LOCK TABLES `villages` WRITE;
/*!40000 ALTER TABLE `villages` DISABLE KEYS */;
/*!40000 ALTER TABLE `villages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `warehouse`
--

DROP TABLE IF EXISTS `warehouse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `warehouse` (
  `WareHouseInTranID` int NOT NULL AUTO_INCREMENT,
  `ProductID` varchar(15) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `UpdatedDate` datetime DEFAULT NULL,
  `StockCount` double DEFAULT NULL,
  `GoodSize` varchar(45) DEFAULT NULL,
  `UserID` int DEFAULT NULL,
  `OtherColumn1` varchar(45) DEFAULT NULL,
  `OtherColumn2` varchar(45) DEFAULT NULL,
  `OtherColoum3` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`WareHouseInTranID`),
  KEY `WareHouseInToProduct_idx` (`ProductID`),
  KEY `WareHouseInToUser_idx` (`UserID`),
  CONSTRAINT `WareHouseInToProduct` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `WareHouseInToUser` FOREIGN KEY (`UserID`) REFERENCES `user` (`UserID`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `warehouse`
--

LOCK TABLES `warehouse` WRITE;
/*!40000 ALTER TABLE `warehouse` DISABLE KEYS */;
/*!40000 ALTER TABLE `warehouse` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-12-04 20:58:07
