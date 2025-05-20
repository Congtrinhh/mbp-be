-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: mbp
-- ------------------------------------------------------
-- Server version	8.0.40

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

--
-- Table structure for table `client_review_mc`
--

DROP TABLE IF EXISTS `client_review_mc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client_review_mc` (
  `id` int NOT NULL AUTO_INCREMENT,
  `client_id` int DEFAULT NULL,
  `mc_id` int DEFAULT NULL,
  `contract_id` int DEFAULT NULL,
  `pro_point` tinyint NOT NULL DEFAULT '5' COMMENT 'khách hàng chấm điểm trình độ chuyên môn của MC: thang điểm từ 1 đến 5',
  `attitude_point` tinyint NOT NULL DEFAULT '5' COMMENT 'khách hàng chấm thái độ làm việc của MC: thang điểm từ 1 đến 5',
  `short_description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'tiêu đề của bài post',
  `detail_description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'nội dung của bài post',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  `overall_point` tinyint NOT NULL DEFAULT '5',
  `reliable_point` tinyint NOT NULL DEFAULT '5',
  PRIMARY KEY (`id`),
  KEY `contract_id` (`contract_id`),
  CONSTRAINT `client_review_mc_ibfk_3` FOREIGN KEY (`contract_id`) REFERENCES `contract` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='bài post khách hàng đánh giá về mc';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client_review_mc`
--

LOCK TABLES `client_review_mc` WRITE;
/*!40000 ALTER TABLE `client_review_mc` DISABLE KEYS */;
INSERT INTO `client_review_mc` VALUES (1,NULL,NULL,NULL,4,4,'a','It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using \'Content here, content here\', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for \'lorem ipsum\' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).','2025-02-02 08:38:05','2025-02-02 17:59:06',NULL,NULL,_binary '',4,4),(24,62,58,22,5,5,'Lần sau sẽ hợp tác lại với bạn',NULL,'2025-05-13 13:20:05','2025-05-13 13:20:05',62,NULL,_binary '',5,5),(25,62,58,24,5,4,'Rất tốt','Sẽ hợp tác lại lần sau','2025-05-14 07:04:49','2025-05-14 07:04:49',62,NULL,_binary '',5,4);
/*!40000 ALTER TABLE `client_review_mc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contract`
--

DROP TABLE IF EXISTS `contract`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contract` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mc_id` int NOT NULL,
  `client_id` int NOT NULL,
  `event_start` timestamp NOT NULL COMMENT 'thời gian MC bắt đầu dẫn chương trình (bắt đầu ca dẫn)',
  `event_end` timestamp NOT NULL COMMENT 'thời gian MC kết thúc dẫn chương trình (xong việc)',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'Mô tả về chương trình',
  `place` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'địa điểm sự kiện diễn ra',
  `mc_cancel_date` datetime DEFAULT NULL COMMENT 'thời gian MC hủy hợp đồng. nếu trường này có giá trị tức là hợp đồng này bị hủy',
  `mc_cancel_reason` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'lý do MC hủy hợp đồng',
  `client_cancel_date` datetime DEFAULT NULL COMMENT 'thời gian khách book MC hủy hợp đồng. nếu trường này có giá trị tức là hợp đồng này bị hủy',
  `client_cancel_reason` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'lý do khách book MC hủy hợp đồng',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  `event_name` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `status` tinyint NOT NULL DEFAULT '1' COMMENT 'trạng thái hợp đồng. 1: có hiệu lực; 2: bị hủy; 3: đã hoàn thành',
  `is_remind` bit(1) NOT NULL DEFAULT b'0' COMMENT 'Đã gửi nhắc nhở đánh giá hay chưa',
  PRIMARY KEY (`id`),
  KEY `idx_mc_id` (`mc_id`),
  KEY `fk_contract_client_id_idx` (`client_id`),
  CONSTRAINT `fk_contract_client_id` FOREIGN KEY (`client_id`) REFERENCES `user` (`id`),
  CONSTRAINT `fk_contract_mc_id` FOREIGN KEY (`mc_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Thỏa thuận dẫn chương trình giữa khách và MC';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contract`
--

LOCK TABLES `contract` WRITE;
/*!40000 ALTER TABLE `contract` DISABLE KEYS */;
INSERT INTO `contract` VALUES (19,58,62,'2025-06-01 01:22:00','2025-06-01 04:22:00',NULL,'Trường đại học Công nghiệp Hà Nội ',NULL,NULL,'2025-05-11 11:19:29','Hết nhu cầu','2025-05-11 11:18:58','2025-05-11 11:19:28',58,62,_binary '','Sự kiện Tết thiếu nhi 1-6 DCN',2,_binary '\0'),(21,58,62,'2025-06-01 06:00:14','2025-06-01 08:00:14','Mức cát-xê thương lượng','Trường đại học Công nghiệp Hà Nội',NULL,NULL,NULL,NULL,'2025-05-13 12:37:16','2025-05-13 12:37:16',58,NULL,_binary '','Sự kiện Tết thiếu nhi 1-6 40',1,_binary '\0'),(22,58,62,'2025-06-01 08:20:14','2025-06-10 09:15:14','Mức cát-xê thương lượng','Trường đại học Công nghiệp Hà Nội',NULL,NULL,NULL,NULL,'2025-05-13 12:41:34','2025-05-13 12:41:34',58,NULL,_binary '','Sự kiện Tết thiếu nhi 1-6 42',1,_binary '\0'),(23,58,62,'2025-05-13 08:22:14','2025-05-13 08:23:14','Mức cát-xê thương lượng','Trường đại học Công nghiệp Hà Nội',NULL,NULL,NULL,NULL,'2025-05-13 15:21:27','2025-05-13 15:24:03',58,NULL,_binary '','Sự kiện Tết thiếu nhi 1-6 43',3,_binary ''),(24,58,62,'2025-05-13 23:18:14','2025-05-13 23:19:14','Mức cát-xê thương lượng','Trường đại học Công nghiệp Hà Nội',NULL,NULL,NULL,NULL,'2025-05-14 06:17:11','2025-05-14 06:21:29',58,NULL,_binary '','Sự kiện Tết thiếu nhi mầm non Sen vàng',3,_binary '');
/*!40000 ALTER TABLE `contract` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee` (
  `id` int NOT NULL AUTO_INCREMENT,
  `full_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Họ tên admin',
  `user_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Tên tài khoản đăng nhập cho admin',
  `password_hash` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Mật khẩu dạng băm (đã mã hóa) ',
  `is_admin` bit(1) DEFAULT b'0' COMMENT 'lưu dư thừa - là admin hay không',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Các người dùng của trang quản trị';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee_role`
--

DROP TABLE IF EXISTS `employee_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee_role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `role_id` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `employee_id` (`employee_id`),
  KEY `role_id` (`role_id`),
  CONSTRAINT `employee_role_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`id`) ON DELETE CASCADE,
  CONSTRAINT `employee_role_ibfk_2` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee_role`
--

LOCK TABLES `employee_role` WRITE;
/*!40000 ALTER TABLE `employee_role` DISABLE KEYS */;
/*!40000 ALTER TABLE `employee_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hosting_style`
--

DROP TABLE IF EXISTS `hosting_style`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hosting_style` (
  `id` int NOT NULL AUTO_INCREMENT,
  `label` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Phong cách dẫn chương trình của MC';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hosting_style`
--

LOCK TABLES `hosting_style` WRITE;
/*!40000 ALTER TABLE `hosting_style` DISABLE KEYS */;
INSERT INTO `hosting_style` VALUES (16,'Nhẹ nhàng','2024-12-31 06:11:50','2024-12-31 06:11:50',1,1,_binary ''),(17,'Nhiệt huyết','2024-12-31 06:11:57','2024-12-31 06:11:57',1,1,_binary ''),(18,'Nhanh','2024-12-31 06:12:05','2024-12-31 06:12:05',1,1,_binary ''),(19,'Tốc độ vừa phải','2024-12-31 06:12:06','2024-12-31 06:12:06',1,1,_binary ''),(20,'Hài hước','2024-12-31 06:12:09','2024-12-31 06:16:24',1,1,_binary '');
/*!40000 ALTER TABLE `hosting_style` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `id_info`
--

DROP TABLE IF EXISTS `id_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `id_info` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'ID bản ghi',
  `user_id` int NOT NULL COMMENT 'ID người dùng, liên kết với bảng user',
  `id_number` varchar(100) DEFAULT NULL COMMENT 'Số hiệu CMT/CCCD',
  `name` varchar(255) DEFAULT NULL COMMENT 'Họ tên theo CMT/CCCD',
  `dob` date DEFAULT NULL COMMENT 'Ngày sinh (dob)',
  `sex` varchar(20) DEFAULT NULL COMMENT 'Giới tính',
  `nationality` varchar(50) DEFAULT NULL COMMENT 'Quốc tịch',
  `home` varchar(255) DEFAULT NULL COMMENT 'Quê quán hoặc nơi thường trú',
  `address` varchar(512) DEFAULT NULL COMMENT 'Địa chỉ cư trú',
  `doe` date DEFAULT NULL COMMENT 'Ngày hết hạn (doe)',
  `religion` varchar(50) DEFAULT NULL COMMENT 'Tôn giáo (thông tin mặt sau)',
  `ethnicity` varchar(50) DEFAULT NULL COMMENT 'Dân tộc (thông tin mặt sau)',
  `features` text COMMENT 'Đặc điểm nhận dạng (features) từ mặt sau',
  `issue_date` date DEFAULT NULL COMMENT 'Ngày cấp CMT/CCCD (thông tin mặt sau)',
  `issue_loc` varchar(255) DEFAULT NULL COMMENT 'Nơi cấp CMT/CCCD (thông tin mặt sau)',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `id_info_ibfk_1` (`user_id`),
  CONSTRAINT `id_info_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Bảng lưu thông tin xác minh ID, hợp nhất dữ liệu từ cả mặt trước và mặt sau của CMT/CCCD';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `id_info`
--

LOCK TABLES `id_info` WRITE;
/*!40000 ALTER TABLE `id_info` DISABLE KEYS */;
INSERT INTO `id_info` VALUES (5,58,'010201008414','TRỊNH QUÝ CÔNG','2001-01-03','NAM','VIỆT NAM','QUẢNG PHÚC, QUẢNG XƯƠNG, THANH HÓA','THÔN SƠN LẦU, CAM ĐƯỜNG, THÀNH PHỐ LÀO CAI, LÀO CAI','2026-01-03',NULL,NULL,'NỐT RUỒI ĐẦU MẮT TRÁI','2022-02-20','CỤC CẢNH SÁT QUẢN LÝ HÀNH CHÍNH VỀ TRẬT TỰ XÃ HỘI','2025-05-14 07:01:25','2025-05-14 07:01:25',NULL,NULL,_binary '');
/*!40000 ALTER TABLE `id_info` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mc_hosting_style`
--

DROP TABLE IF EXISTS `mc_hosting_style`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mc_hosting_style` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mc_id` int NOT NULL,
  `hosting_style_id` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `idx_mc_id` (`mc_id`),
  KEY `idx_hosting_style_id` (`hosting_style_id`),
  CONSTRAINT `fk_MC_host_manner_mc_id` FOREIGN KEY (`mc_id`) REFERENCES `user` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_mc_hosting_style_hosting_style_id` FOREIGN KEY (`hosting_style_id`) REFERENCES `hosting_style` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mc_hosting_style`
--

LOCK TABLES `mc_hosting_style` WRITE;
/*!40000 ALTER TABLE `mc_hosting_style` DISABLE KEYS */;
INSERT INTO `mc_hosting_style` VALUES (28,58,20,'2025-01-30 23:50:31','2025-01-30 23:50:31',NULL,NULL,_binary ''),(29,58,19,'2025-01-30 23:50:31','2025-01-30 23:50:31',NULL,NULL,_binary ''),(30,58,17,'2025-03-23 04:39:55','2025-03-23 04:39:55',NULL,NULL,_binary ''),(31,67,16,'2025-05-18 04:03:24','2025-05-18 04:03:24',NULL,NULL,_binary ''),(32,67,20,'2025-05-18 04:03:24','2025-05-18 04:03:24',NULL,NULL,_binary ''),(33,68,19,'2025-05-18 04:09:18','2025-05-18 04:09:18',NULL,NULL,_binary ''),(34,68,16,'2025-05-18 04:09:18','2025-05-18 04:09:18',NULL,NULL,_binary ''),(35,68,17,'2025-05-18 04:09:18','2025-05-18 04:09:18',NULL,NULL,_binary ''),(36,69,20,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary ''),(37,69,16,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary ''),(38,69,17,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary '');
/*!40000 ALTER TABLE `mc_hosting_style` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mc_mc_type`
--

DROP TABLE IF EXISTS `mc_mc_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mc_mc_type` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mc_id` int NOT NULL,
  `mc_type_id` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `idx_mc_id` (`mc_id`),
  KEY `idx_mc_type_id` (`mc_type_id`),
  CONSTRAINT `fk_mc_type_mc_mc_type` FOREIGN KEY (`mc_type_id`) REFERENCES `mc_type` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_user_mc_mc_type` FOREIGN KEY (`mc_id`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mc_mc_type`
--

LOCK TABLES `mc_mc_type` WRITE;
/*!40000 ALTER TABLE `mc_mc_type` DISABLE KEYS */;
INSERT INTO `mc_mc_type` VALUES (32,58,4,'2025-03-23 04:28:25','2025-03-23 04:28:25',NULL,NULL,_binary ''),(33,58,2,'2025-03-23 04:28:25','2025-03-23 04:28:25',NULL,NULL,_binary ''),(35,58,3,'2025-05-18 00:43:21','2025-05-18 00:43:21',NULL,NULL,_binary ''),(36,67,3,'2025-05-18 04:03:24','2025-05-18 04:03:24',NULL,NULL,_binary ''),(37,67,1,'2025-05-18 04:03:24','2025-05-18 04:03:24',NULL,NULL,_binary ''),(38,68,3,'2025-05-18 04:09:18','2025-05-18 04:09:18',NULL,NULL,_binary ''),(39,68,4,'2025-05-18 04:09:18','2025-05-18 04:09:18',NULL,NULL,_binary ''),(40,69,4,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary ''),(41,69,3,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary ''),(42,69,2,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary '');
/*!40000 ALTER TABLE `mc_mc_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mc_province`
--

DROP TABLE IF EXISTS `mc_province`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mc_province` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mc_id` int NOT NULL,
  `province_id` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `idx_mc_id` (`mc_id`),
  KEY `idx_province_id` (`province_id`),
  CONSTRAINT `fk_province_mc_province` FOREIGN KEY (`province_id`) REFERENCES `province` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_user_mc_province` FOREIGN KEY (`mc_id`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=137 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mc_province`
--

LOCK TABLES `mc_province` WRITE;
/*!40000 ALTER TABLE `mc_province` DISABLE KEYS */;
INSERT INTO `mc_province` VALUES (128,58,1,'2025-01-30 23:50:31','2025-01-30 23:50:31',NULL,NULL,_binary ''),(129,58,2,'2025-03-22 08:32:32','2025-03-22 08:32:32',NULL,NULL,_binary ''),(131,67,1,'2025-05-18 04:03:24','2025-05-18 04:03:24',NULL,NULL,_binary ''),(132,68,1,'2025-05-18 04:09:18','2025-05-18 04:09:18',NULL,NULL,_binary ''),(133,68,23,'2025-05-18 04:09:18','2025-05-18 04:09:18',NULL,NULL,_binary ''),(134,69,1,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary ''),(135,69,2,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary ''),(136,69,3,'2025-05-18 04:21:30','2025-05-18 04:21:30',NULL,NULL,_binary '');
/*!40000 ALTER TABLE `mc_province` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mc_review_client`
--

DROP TABLE IF EXISTS `mc_review_client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mc_review_client` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mc_id` int DEFAULT NULL,
  `client_id` int DEFAULT NULL,
  `contract_id` int DEFAULT NULL,
  `payment_punctual_point` int NOT NULL COMMENT 'mc chấm điểm thanh toán cát xê đúng hạn của khách: thang điểm từ 1 đến 5',
  `short_description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'tiêu đề của bài post',
  `detail_description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'nội dung của bài post',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  `overall_point` tinyint DEFAULT '5',
  `reliable_point` tinyint DEFAULT '5',
  PRIMARY KEY (`id`),
  KEY `contract_id` (`contract_id`),
  CONSTRAINT `mc_review_client_ibfk_3` FOREIGN KEY (`contract_id`) REFERENCES `contract` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='bài post mc đánh giá về khách hàng';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mc_review_client`
--

LOCK TABLES `mc_review_client` WRITE;
/*!40000 ALTER TABLE `mc_review_client` DISABLE KEYS */;
INSERT INTO `mc_review_client` VALUES (12,58,62,22,4,'Lần sau cần chau chuốt ngôn từ dẫn hơn',NULL,'2025-05-13 13:19:02','2025-05-13 13:19:02',58,NULL,_binary '',5,4),(13,58,62,24,5,'Rất vui đc hợp tác với anh',NULL,'2025-05-14 07:05:24','2025-05-14 07:05:24',58,NULL,_binary '',5,4);
/*!40000 ALTER TABLE `mc_review_client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mc_type`
--

DROP TABLE IF EXISTS `mc_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mc_type` (
  `id` int NOT NULL AUTO_INCREMENT,
  `label` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Loại MC: mc gala dinner, mc cưới,..';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mc_type`
--

LOCK TABLES `mc_type` WRITE;
/*!40000 ALTER TABLE `mc_type` DISABLE KEYS */;
INSERT INTO `mc_type` VALUES (1,'MC Gala Dinner','MC chuyên dẫn chương trình gala dinner','2024-12-31 06:26:12','2024-12-31 06:26:12',1,1,_binary ''),(2,'MC Cưới','MC chuyên dẫn chương trình đám cưới','2024-12-31 06:26:23','2024-12-31 06:26:23',1,1,_binary ''),(3,'MC Hội Nghị','MC chuyên dẫn chương trình hội nghị','2024-12-31 06:26:23','2024-12-31 06:26:23',1,1,_binary ''),(4,'MC Sự Kiện','MC chuyên dẫn chương trình sự kiện','2024-12-31 06:26:23','2024-12-31 06:26:23',1,1,_binary ''),(5,'MC Truyền Hình','MC chuyên dẫn chương trình truyền hình','2024-12-31 06:26:23','2024-12-31 06:26:23',1,1,_binary '');
/*!40000 ALTER TABLE `mc_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `media`
--

DROP TABLE IF EXISTS `media`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `media` (
  `id` int NOT NULL AUTO_INCREMENT,
  `type` tinyint NOT NULL DEFAULT '0' COMMENT 'loại file: 0 - ko phải file; 1 - ảnh; 2 - video; 3 - audio',
  `url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'url để lấy ảnh/video từ nhà cung cấp',
  `user_id` int NOT NULL,
  `sort_order` int NOT NULL DEFAULT '0' COMMENT 'thứ tự sắp xếp của media trên profile MC, số lớn nhất xếp ở đầu. trong 1 danh sách ảnh/video, cái đầu tiên bắt đầu từ 1',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `fk_mc_media_idx` (`user_id`),
  CONSTRAINT `fk_mc_media` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='ảnh và video của MC';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `media`
--

LOCK TABLES `media` WRITE;
/*!40000 ALTER TABLE `media` DISABLE KEYS */;
INSERT INTO `media` VALUES (35,1,'https://mc-booking-platform.s3.amazonaws.com/images/9f7b514f-e633-4a97-8cfe-f83932a60969',58,2,'2025-05-14 07:02:55','2025-05-18 00:50:19',58,NULL,_binary ''),(36,1,'https://mc-booking-platform.s3.amazonaws.com/images/80706cee-f26b-4b1e-b039-b6dcd364589f',58,3,'2025-05-14 07:03:15','2025-05-18 00:50:19',58,NULL,_binary ''),(37,1,'https://mc-booking-platform.s3.amazonaws.com/images/43f43374-a3d1-4e7c-8ef8-d054e3896bfc',58,4,'2025-05-18 00:39:46','2025-05-18 00:50:19',58,NULL,_binary ''),(38,1,'https://mc-booking-platform.s3.amazonaws.com/images/10360e8a-9b22-4959-9645-33d2bd25398d',58,5,'2025-05-18 00:49:27','2025-05-18 00:50:19',58,NULL,_binary ''),(39,1,'https://mc-booking-platform.s3.amazonaws.com/images/d087d115-76ac-4bd5-8546-b82efbc10cb6',58,1,'2025-05-18 00:49:50','2025-05-18 00:50:19',58,NULL,_binary ''),(40,2,'https://mc-booking-platform.s3.amazonaws.com/videos/96faf3a7-7734-4a03-8da8-798c7d633fd3',58,1,'2025-05-18 00:49:57','2025-05-18 00:49:57',58,NULL,_binary ''),(41,1,'https://mc-booking-platform.s3.amazonaws.com/images/cf32e102-f7bd-4c3d-85eb-9b188457913f',69,1,'2025-05-18 04:22:03','2025-05-18 04:22:03',69,NULL,_binary ''),(42,1,'https://mc-booking-platform.s3.amazonaws.com/images/02c94bbd-e774-403b-aa5d-8320ece018a3',69,1,'2025-05-18 04:22:05','2025-05-18 04:22:05',69,NULL,_binary '');
/*!40000 ALTER TABLE `media` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `message`
--

DROP TABLE IF EXISTS `message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `message` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mc_id` int NOT NULL,
  `client_id` int NOT NULL,
  `content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'nội dung text của tin nhắn, nếu tin nhắn là media thì giá trị trường này là null',
  `sent_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'tin nhắn được gửi khi nào?',
  `revoked_at` timestamp NULL DEFAULT NULL COMMENT 'tin nhắn được thu hồi khi nào?',
  `media_url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'link lấy ảnh/video/audio của tin nhắn từ nhà cung cấp',
  `media_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT '0' COMMENT 'loại file: 0 - ko phải file; 1 - ảnh; 2 - video; 3 - audio',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `idx_mc_id` (`mc_id`),
  KEY `fk_message_client_id_idx` (`client_id`),
  CONSTRAINT `fk_message_client_id` FOREIGN KEY (`client_id`) REFERENCES `user` (`id`),
  CONSTRAINT `fk_message_mc_id` FOREIGN KEY (`mc_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='tin nhắn giữa MC và khách hàng';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `message`
--

LOCK TABLES `message` WRITE;
/*!40000 ALTER TABLE `message` DISABLE KEYS */;
/*!40000 ALTER TABLE `message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notification`
--

DROP TABLE IF EXISTS `notification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notification` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_id` int DEFAULT NULL,
  `message` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT '',
  `is_read` bit(1) DEFAULT b'0',
  `additional_info` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1',
  `type` int NOT NULL COMMENT 'loại thông báo - dùng để xử lý điều hướng, logic khi bấm vào thông báo',
  `thumb_url` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL COMMENT 'image to display in the notification ui',
  `status` tinyint DEFAULT '2' COMMENT 'trạn tái của tôn báo: 0 - ko thể thao tác các action phía sau khi bấm vào thông báo ; 2 - có htể thao tác các action phía sau khi bấm vào thông báo',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=160 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='thông báo quả chuông';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notification`
--

LOCK TABLES `notification` WRITE;
/*!40000 ALTER TABLE `notification` DISABLE KEYS */;
INSERT INTO `notification` VALUES (37,58,'You have received a new offer',_binary '','{\"eventName\":\"GDE 2025 - 3\",\"eventStart\":\"2025-02-08T11:14:12.720Z\",\"eventEnd\":\"2025-02-08T13:14:12.720Z\",\"place\":\"Mavin sleek\",\"note\":\"Dẫn tốt nhiều cơ hội\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 02:15:04','2025-02-08 02:15:26',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(38,59,'Offer cho sự kiện GDE 2025 - 3 của bạn đã bị từ chối.',_binary '','{\"notificationId\":37}','2025-02-08 02:15:26','2025-02-08 02:15:37',NULL,NULL,_binary '',2,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(39,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"GDE 2025 - 3\",\"eventStart\":\"2025-02-08T11:14:12.720Z\",\"eventEnd\":\"2025-02-08T13:14:12.720Z\",\"place\":\"Mavin sleek\",\"note\":\"Tăng cát xê thêm 50%\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 02:15:51','2025-02-08 02:18:15',NULL,NULL,_binary '',1,NULL,0),(40,59,'Offer cho sự kiện GDE 2025 - 3 của bạn đã bị từ chối.',_binary '','{\"notificationId\":39}','2025-02-08 02:18:14','2025-02-08 03:03:52',NULL,NULL,_binary '',2,NULL,2),(41,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"GDE 2025 - 3\",\"eventStart\":\"2025-02-08T11:14:12.720Z\",\"eventEnd\":\"2025-02-08T13:14:12.720Z\",\"place\":\"Mavin sleek\",\"note\":\"X3 catse\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 02:18:29','2025-02-08 02:18:43',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(42,59,'Offer cho sự kiện GDE 2025 - 3 của bạn đã được chấp nhận.',_binary '',NULL,'2025-02-08 02:18:42','2025-02-08 02:18:50',NULL,NULL,_binary '',3,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(43,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"GDE 2025 - 4\",\"eventStart\":\"2025-02-08T11:14:12.720Z\",\"eventEnd\":\"2025-02-08T13:14:12.720Z\",\"place\":\"Crystal palace\",\"note\":\"Dẫn tốt nhiều cơ hội oke\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 02:25:11','2025-02-08 02:25:20',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(44,59,'Offer cho sự kiện GDE 2025 - 4 của bạn đã được chấp nhận.',_binary '',NULL,'2025-02-08 02:25:19','2025-02-08 02:28:37',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(45,58,'Bạn đã nhận được một offer mới',_binary '\0','{\"eventName\":\"GDE 2025 - 3\",\"eventStart\":\"2025-02-10T11:14:12.000Z\",\"eventEnd\":\"2025-02-11T13:14:12.000Z\",\"place\":\"Mavin sleek\",\"note\":\"x4 nha\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 19:23:47','2025-02-08 19:23:47',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(46,58,'Bạn đã nhận được một offer mới',_binary '\0','{\"eventName\":\"GDE 2025 - 3\",\"eventStart\":\"2025-02-10T11:14:12.000Z\",\"eventEnd\":\"2025-02-11T13:14:12.000Z\",\"place\":\"Mavin sleek\",\"note\":\"x5\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 19:30:57','2025-02-08 19:30:57',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(47,58,'You have received a new offer',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-09T03:35:27.335Z\",\"eventEnd\":\"2025-02-09T04:35:27.335Z\",\"place\":\"Lautaro royal\",\"note\":\"1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 19:36:13','2025-02-08 19:36:37',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(48,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã bị từ chối.',_binary '','{\"notificationId\":47}','2025-02-08 19:36:36','2025-02-08 19:36:52',NULL,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(49,58,'Bạn đã nhận được một offer mới',_binary '\0','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-09T03:35:27.335Z\",\"eventEnd\":\"2025-02-09T04:35:27.335Z\",\"place\":\"Lautaro royal\",\"note\":\"2 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 19:37:04','2025-02-08 19:37:04',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(50,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-09T03:35:27.335Z\",\"eventEnd\":\"2025-02-09T04:35:27.335Z\",\"place\":\"Lautaro royal\",\"note\":\"2,5 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 19:46:59','2025-02-08 19:47:17',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(51,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã được chấp nhận.',_binary '',NULL,'2025-02-08 19:47:17','2025-02-08 20:06:17',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(52,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-09T03:35:27.335Z\",\"eventEnd\":\"2025-02-09T04:35:27.335Z\",\"place\":\"Lautaro royal\",\"note\":\"3 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 20:06:30','2025-02-08 20:08:57',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(53,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-09T03:35:27.335Z\",\"eventEnd\":\"2025-02-09T04:35:27.335Z\",\"place\":\"Lautaro royal\",\"note\":\"2.1v triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 20:07:44','2025-02-08 20:36:00',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(54,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-09T03:35:27.335Z\",\"eventEnd\":\"2025-02-09T04:35:27.335Z\",\"place\":\"Lautaro royal\",\"note\":\"2.2 triệu oke\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 20:08:17','2025-02-08 20:33:11',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(55,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-09T03:35:27.335Z\",\"eventEnd\":\"2025-02-09T04:35:27.335Z\",\"place\":\"Lautaro royal\",\"note\":\"3.1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-08 20:17:24','2025-02-08 20:17:29',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',2),(56,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã bị từ chối.',_binary '','{\"notificationId\":53}','2025-02-08 20:36:00','2025-02-11 06:55:00',NULL,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(57,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-16T03:35:27.000Z\",\"eventEnd\":\"2025-02-17T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"3.5 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:00:56','2025-02-15 08:20:15',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(58,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-17T03:35:27.000Z\",\"eventEnd\":\"2025-02-18T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"4 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:01:36','2025-02-15 08:18:08',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(59,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã được chấp nhận.',_binary '',NULL,'2025-02-15 08:18:08','2025-02-15 08:18:20',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(60,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã được chấp nhận.',_binary '',NULL,'2025-02-15 08:20:15','2025-02-15 08:20:23',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(61,59,'The contract on event MC Miền Nam 2025 is canceled.',_binary '','11','2025-02-15 08:20:45','2025-02-15 08:21:00',NULL,NULL,_binary '',5,NULL,0),(62,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-22T03:35:27.000Z\",\"eventEnd\":\"2025-02-23T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"4.1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:32:24','2025-02-15 08:33:01',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(63,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã được chấp nhận.',_binary '',NULL,'2025-02-15 08:33:01','2025-02-15 08:33:17',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(64,58,'The contract on event MC Miền Nam 2025 is canceled.',_binary '','{\"ContractId\":12}','2025-02-15 08:33:26','2025-02-15 08:33:41',NULL,NULL,_binary '',5,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(65,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-22T03:35:27.000Z\",\"eventEnd\":\"2025-02-23T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:39:07','2025-02-15 08:39:16',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(66,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã được chấp nhận.',_binary '',NULL,'2025-02-15 08:39:16','2025-02-15 08:39:46',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(67,58,'The contract on event MC Miền Nam 2025 is canceled.',_binary '','{\"contractId\":13}','2025-02-15 08:40:00','2025-02-15 08:40:09',NULL,NULL,_binary '',5,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(68,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-24T03:35:27.000Z\",\"eventEnd\":\"2025-02-25T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:49:50','2025-02-15 08:49:58',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(69,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã bị từ chối.',_binary '','{\"notificationId\":68}','2025-02-15 08:49:58','2025-02-15 08:50:01',NULL,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(70,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-24T03:35:27.000Z\",\"eventEnd\":\"2025-02-25T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:50:03','2025-02-15 08:50:07',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(71,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã được chấp nhận.',_binary '','{\"contractId\":0}','2025-02-15 08:50:07','2025-02-15 08:57:15',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(72,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-24T03:35:27.000Z\",\"eventEnd\":\"2025-02-25T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:51:48','2025-02-15 08:51:55',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(73,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã bị từ chối.',_binary '','{\"notificationId\":72}','2025-02-15 08:51:55','2025-02-15 08:52:17',NULL,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(74,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"MC Miền Nam 2025\",\"eventStart\":\"2025-02-24T03:35:27.000Z\",\"eventEnd\":\"2025-02-25T04:35:27.000Z\",\"place\":\"Lautaro royal\",\"note\":\"1 triệu\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-15 08:52:18','2025-02-15 08:52:27',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(75,59,'Offer cho sự kiện MC Miền Nam 2025 của bạn đã được chấp nhận.',_binary '','{\"contractId\":15}','2025-02-15 08:52:26','2025-02-15 08:52:36',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(76,59,'The contract on event MC Miền Nam 2025 is canceled.',_binary '','{\"contractId\":15}','2025-02-15 08:57:04','2025-02-15 08:57:07',NULL,NULL,_binary '',5,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',0),(77,58,'Show đã hoàn thành, đánh giá mc nào.',_binary '',NULL,'2025-02-16 14:48:09','2025-02-16 14:49:06',NULL,NULL,_binary '',4,NULL,2),(78,59,'Show đã hoàn thành, đánh giá mc nào.',_binary '',NULL,'2025-02-16 14:49:17','2025-02-16 14:49:25',NULL,NULL,_binary '',4,NULL,2),(79,58,'Vui lòng đánh giá sự kiện \'\' của bạn.',_binary '',NULL,'2025-02-21 13:55:18','2025-05-08 13:17:31',NULL,NULL,_binary '',4,NULL,0),(80,59,'Vui lòng đánh giá sự kiện \'\' của bạn.',_binary '\0',NULL,'2025-02-21 13:55:18','2025-02-21 13:55:18',NULL,NULL,_binary '',4,NULL,0),(81,58,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '',NULL,'2025-02-21 13:55:18','2025-05-13 12:51:01',NULL,58,_binary '',4,NULL,0),(82,59,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '\0',NULL,'2025-02-21 13:55:18','2025-02-21 13:55:18',NULL,NULL,_binary '',4,NULL,0),(83,58,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '',NULL,'2025-02-21 13:55:18','2025-05-13 12:50:22',NULL,58,_binary '',4,NULL,0),(84,59,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '\0',NULL,'2025-02-21 13:55:18','2025-02-21 13:55:18',NULL,NULL,_binary '',4,NULL,0),(85,58,'Vui lòng đánh giá sự kiện \'GDE 2025 - 3\' của bạn.',_binary '',NULL,'2025-02-21 13:55:18','2025-05-13 12:50:12',NULL,58,_binary '',4,NULL,0),(86,59,'Vui lòng đánh giá sự kiện \'GDE 2025 - 3\' của bạn.',_binary '\0',NULL,'2025-02-21 13:55:18','2025-02-21 13:55:18',NULL,NULL,_binary '',4,NULL,0),(87,58,'Vui lòng đánh giá sự kiện \'GDE 2025 - 4\' của bạn.',_binary '',NULL,'2025-02-21 13:55:18','2025-02-21 14:04:52',NULL,NULL,_binary '',4,NULL,0),(88,59,'Vui lòng đánh giá sự kiện \'GDE 2025 - 4\' của bạn.',_binary '\0',NULL,'2025-02-21 13:55:18','2025-02-21 13:55:18',NULL,NULL,_binary '',4,NULL,0),(89,58,'Vui lòng đánh giá sự kiện \'\' của bạn.',_binary '','{\"contractId\":3}','2025-02-23 05:51:50','2025-02-23 05:52:37',NULL,NULL,_binary '',4,NULL,0),(90,59,'Vui lòng đánh giá sự kiện \'\' của bạn.',_binary '\0','{\"contractId\":3}','2025-02-23 05:51:50','2025-02-23 05:51:50',NULL,NULL,_binary '',4,NULL,0),(91,58,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '','{\"contractId\":5}','2025-02-23 05:51:50','2025-02-23 05:52:35',NULL,NULL,_binary '',4,NULL,0),(92,59,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '\0','{\"contractId\":5}','2025-02-23 05:51:50','2025-02-23 05:51:50',NULL,NULL,_binary '',4,NULL,0),(93,58,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '','{\"contractId\":6}','2025-02-23 05:51:50','2025-02-23 05:52:32',NULL,NULL,_binary '',4,NULL,0),(94,59,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '\0','{\"contractId\":6}','2025-02-23 05:51:50','2025-02-23 05:51:50',NULL,NULL,_binary '',4,NULL,0),(95,58,'Vui lòng đánh giá sự kiện \'GDE 2025 - 3\' của bạn.',_binary '','{\"contractId\":7}','2025-02-23 05:51:50','2025-02-23 05:52:29',NULL,NULL,_binary '',4,NULL,0),(96,59,'Vui lòng đánh giá sự kiện \'GDE 2025 - 3\' của bạn.',_binary '\0','{\"contractId\":7}','2025-02-23 05:51:50','2025-02-23 05:51:50',NULL,NULL,_binary '',4,NULL,0),(97,58,'Vui lòng đánh giá sự kiện \'GDE 2025 - 4\' của bạn.',_binary '','{\"contractId\":8}','2025-02-23 05:51:50','2025-02-23 05:52:00',NULL,NULL,_binary '',4,NULL,0),(98,59,'Vui lòng đánh giá sự kiện \'GDE 2025 - 4\' của bạn.',_binary '\0','{\"contractId\":8}','2025-02-23 05:51:50','2025-02-23 05:51:50',NULL,NULL,_binary '',4,NULL,0),(99,58,'Vui lòng đánh giá sự kiện \'\' của bạn.',_binary '','{\"contractId\":3}','2025-02-23 05:53:50','2025-02-23 05:56:29',NULL,NULL,_binary '',4,NULL,0),(100,59,'Vui lòng đánh giá sự kiện \'\' của bạn.',_binary '\0','{\"contractId\":3}','2025-02-23 05:53:50','2025-02-23 05:53:50',NULL,NULL,_binary '',4,NULL,0),(101,58,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '','{\"contractId\":5}','2025-02-23 05:53:50','2025-05-08 13:17:36',NULL,NULL,_binary '',4,NULL,0),(102,59,'Vui lòng đánh giá sự kiện \'Gấu vàng 2025\' của bạn.',_binary '','{\"contractId\":5}','2025-02-23 05:53:50','2025-02-23 05:57:54',NULL,NULL,_binary '',4,NULL,0),(103,58,'Vui lòng đánh giá sự kiện \'MC Miền Nam 2025\' của bạn.',_binary '','{\"contractId\":14}','2025-02-28 14:12:37','2025-03-23 04:06:46',NULL,NULL,_binary '',4,NULL,0),(104,59,'Vui lòng đánh giá sự kiện \'MC Miền Nam 2025\' của bạn.',_binary '','{\"contractId\":14}','2025-02-28 14:12:37','2025-03-23 04:08:20',NULL,NULL,_binary '',4,NULL,0),(105,58,'Vui lòng đánh giá sự kiện \'MC Miền Nam 2025\' của bạn.',_binary '','{\"contractId\":15}','2025-02-28 14:12:37','2025-03-23 08:09:27',NULL,NULL,_binary '',4,NULL,0),(106,59,'Vui lòng đánh giá sự kiện \'MC Miền Nam 2025\' của bạn.',_binary '\0','{\"contractId\":15}','2025-02-28 14:12:37','2025-02-28 14:12:37',NULL,NULL,_binary '',4,NULL,0),(107,58,'You have received a new offer',_binary '','{\"eventName\":\"GDE 2025 lần 3\",\"eventStart\":\"2025-03-01T14:27:18.000Z\",\"eventEnd\":\"2025-03-02T14:27:18.000Z\",\"place\":\"TT Sapa\",\"note\":\"\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-28 14:29:02','2025-02-28 14:29:44',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(108,59,'Offer cho sự kiện GDE 2025 lần 3 của bạn đã bị từ chối.',_binary '','{\"notificationId\":107}','2025-02-28 14:29:44','2025-02-28 14:30:31',NULL,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(109,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"GDE 2025 lần 3\",\"eventStart\":\"2025-03-01T14:27:18.000Z\",\"eventEnd\":\"2025-03-02T14:27:18.000Z\",\"place\":\"TT Sapa\",\"note\":\"giá 3tr\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-02-28 14:30:43','2025-02-28 14:35:54',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(110,59,'Offer cho sự kiện GDE 2025 lần 3 của bạn đã được chấp nhận.',_binary '\0','{\"contractId\":16}','2025-02-28 14:35:54','2025-02-28 14:35:54',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(111,58,'The contract on event GDE 2025 lần 3 is canceled.',_binary '','{\"contractId\":16}','2025-02-28 14:36:49','2025-03-23 08:11:33',NULL,NULL,_binary '',5,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(112,58,'You have received a new offer',_binary '','{\"eventName\":\"tả van\",\"eventStart\":\"2025-03-29T08:14:02.000Z\",\"eventEnd\":\"2025-04-05T08:14:02.000Z\",\"place\":\"\",\"note\":\"\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-03-23 08:14:35','2025-03-23 08:14:59',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocIDvmVbZZpU_LTNU5gxnt3iqnFpCVvW3dzYB-XPQhLAc5I8TA=s96-c',0),(113,59,'Offer cho sự kiện tả van của bạn đã bị từ chối.',_binary '','{\"notificationId\":112}','2025-03-23 08:14:59','2025-03-23 08:16:09',NULL,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/56084416-e976-4b30-9355-ed6311d90a8d-doan-quoc-dam-dao-pho3-17089209773831657521094.webp',2),(114,58,'You have received a new offer',_binary '','{\"eventName\":\"Fesstival 2025\",\"eventStart\":\"2025-04-26T07:54:31.000Z\",\"eventEnd\":\"2025-04-26T08:54:31.000Z\",\"place\":\"Hà Nội\",\"note\":\"3tr\",\"senderId\":\"59\",\"senderName\":\"Cong Trinh\"}','2025-04-15 07:55:26','2025-04-15 07:55:42',NULL,NULL,_binary '',1,'https://mc-booking-platform.s3.amazonaws.com/images/147bcfd1-0f90-41e0-9465-a9d3746c4446-IMG_0506.JPEG',0),(115,59,'Offer cho sự kiện Fesstival 2025 của bạn đã được chấp nhận.',_binary '','{\"contractId\":17}','2025-04-15 07:55:42','2025-04-15 07:55:48',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(116,58,'The contract on event Fesstival 2025 is canceled.',_binary '','{\"contractId\":17}','2025-04-15 07:56:11','2025-04-15 07:56:17',NULL,NULL,_binary '',5,'https://mc-booking-platform.s3.amazonaws.com/images/147bcfd1-0f90-41e0-9465-a9d3746c4446-IMG_0506.JPEG',0),(117,58,'You have received a new offer',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-08 13:11:33','2025-05-08 13:12:01',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(118,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 của bạn đã bị từ chối.',_binary '','{\"notificationId\":117}','2025-05-08 13:12:01','2025-05-08 13:12:10',NULL,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(119,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê 3tr\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-08 13:12:21','2025-05-08 13:12:30',NULL,NULL,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(120,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 của bạn đã được chấp nhận.',_binary '','{\"contractId\":18}','2025-05-08 13:12:30','2025-05-08 13:12:40',NULL,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(121,58,'You have received a new offer',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 DCN\",\"eventStart\":\"2025-06-01T08:22:00.000Z\",\"eventEnd\":\"2025-06-01T11:22:00.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội \",\"note\":\"\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-11 08:32:15','2025-05-11 11:19:00',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(122,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 DCN của bạn đã được chấp nhận.',_binary '','{\"contractId\":19}','2025-05-11 11:19:00','2025-05-11 11:19:26',58,62,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(123,58,'The contract on event Sự kiện Tết thiếu nhi 1-6 DCN is canceled.',_binary '','{\"contractId\":19}','2025-05-11 11:19:29','2025-05-11 11:19:38',62,58,_binary '',5,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(124,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 23\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-11 11:20:34','2025-05-11 11:22:46',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(125,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 23 của bạn đã được chấp nhận.',_binary '\0','{}','2025-05-11 11:22:46','2025-05-11 11:22:46',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(126,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 24\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-11 11:23:25','2025-05-11 11:24:46',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(127,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 24 của bạn đã được chấp nhận.',_binary '\0','{}','2025-05-11 11:24:46','2025-05-11 11:24:46',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(128,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 2x5\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-11 11:27:00','2025-05-11 11:27:13',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(129,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 2x5 của bạn đã được chấp nhận.',_binary '\0','{}','2025-05-11 11:27:13','2025-05-11 11:27:13',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(130,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 26\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-11 11:29:00','2025-05-11 11:30:10',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(131,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 26 của bạn đã được chấp nhận.',_binary '\0','{}','2025-05-11 11:29:56','2025-05-11 11:29:56',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(132,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 26\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-12 15:02:41','2025-05-12 15:02:45',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',2),(133,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 3e\",\"eventStart\":\"2025-06-01T16:10:14.000Z\",\"eventEnd\":\"2025-06-10T16:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-12 15:23:45','2025-05-12 15:25:07',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(134,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 3e của bạn đã được chấp nhận.',_binary '\0','{\"contractId\":20}','2025-05-12 15:25:00','2025-05-12 15:25:00',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(135,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 3x\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-12 15:25:23','2025-05-12 15:26:26',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(136,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 3x của bạn đã bị từ chối.',_binary '\0','{\"notificationId\":135}','2025-05-12 15:26:26','2025-05-12 15:26:26',58,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(137,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-13 12:24:25','2025-05-13 12:35:59',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(138,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 của bạn đã bị từ chối.',_binary '','{\"notificationId\":137}','2025-05-13 12:35:59','2025-05-14 06:15:58',58,62,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(139,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 40\",\"eventStart\":\"2025-06-01T13:00:14.000Z\",\"eventEnd\":\"2025-06-01T15:00:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-13 12:36:52','2025-05-13 12:37:16',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(140,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 40 của bạn đã được chấp nhận.',_binary '\0','{\"contractId\":21}','2025-05-13 12:37:16','2025-05-13 12:37:16',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(141,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-10T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-13 12:37:25','2025-05-13 12:37:51',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(142,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 của bạn đã bị từ chối.',_binary '','{\"notificationId\":141}','2025-05-13 12:37:51','2025-05-13 15:20:19',58,62,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(143,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 41\",\"eventStart\":\"2025-06-01T13:05:14.000Z\",\"eventEnd\":\"2025-06-10T16:20:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-13 12:38:25','2025-05-13 12:39:08',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(144,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 41 của bạn đã bị từ chối.',_binary '\0','{\"notificationId\":143}','2025-05-13 12:39:08','2025-05-13 12:39:08',58,NULL,_binary '',2,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(145,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 42\",\"eventStart\":\"2025-06-01T15:20:14.000Z\",\"eventEnd\":\"2025-06-10T16:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-13 12:39:37','2025-05-13 12:41:34',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(146,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 42 của bạn đã được chấp nhận.',_binary '\0','{\"contractId\":22}','2025-05-13 12:41:34','2025-05-13 12:41:34',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(147,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6\",\"eventStart\":\"2025-06-01T13:10:14.000Z\",\"eventEnd\":\"2025-06-01T15:15:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-13 12:44:52','2025-05-13 12:44:55',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',2),(148,58,'Vui lòng đánh giá sự kiện \'MC funk\' của bạn.',_binary '','{\"contractId\":22}','2025-05-13 14:12:37','2025-05-13 13:19:02',NULL,58,_binary '',4,NULL,0),(149,62,'Vui lòng đánh giá sự kiện \'MC funk\' của bạn.',_binary '','{\"contractId\":22}','2025-05-13 14:12:37','2025-05-13 13:20:05',NULL,62,_binary '',4,NULL,0),(150,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi 1-6 43\",\"eventStart\":\"2025-05-13T15:22:14.000Z\",\"eventEnd\":\"2025-05-13T15:23:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-13 15:21:21','2025-05-13 15:21:27',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(151,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi 1-6 43 của bạn đã được chấp nhận.',_binary '\0','{\"contractId\":23}','2025-05-13 15:21:27','2025-05-13 15:21:27',58,NULL,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(152,58,'Vui lòng đánh giá sự kiện \'Sự kiện Tết thiếu nhi 1-6 43\' của bạn.',_binary '','{\"contractId\":23}','2025-05-13 15:24:02','2025-05-13 15:24:30',NULL,58,_binary '',4,NULL,2),(153,62,'Vui lòng đánh giá sự kiện \'Sự kiện Tết thiếu nhi 1-6 43\' của bạn.',_binary '','{\"contractId\":23}','2025-05-13 15:24:03','2025-05-13 15:27:18',NULL,62,_binary '',4,NULL,0),(154,58,'Bạn đã nhận được một offer mới',_binary '','{\"eventName\":\"Sự kiện Tết thiếu nhi mầm non Sen vàng\",\"eventStart\":\"2025-05-14T06:18:14.000Z\",\"eventEnd\":\"2025-05-14T06:19:14.000Z\",\"place\":\"Trường đại học Công nghiệp Hà Nội\",\"note\":\"Mức cát-xê thương lượng\",\"senderId\":\"62\",\"senderName\":\"Cong Trinh\"}','2025-05-14 06:17:03','2025-05-14 06:17:11',62,58,_binary '',1,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',0),(155,62,'Offer cho sự kiện Sự kiện Tết thiếu nhi mầm non Sen vàng của bạn đã được chấp nhận.',_binary '','{\"contractId\":24}','2025-05-14 06:17:11','2025-05-14 06:18:28',58,62,_binary '',3,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg',2),(156,58,'Vui lòng đánh giá sự kiện \'Sự kiện Tết thiếu nhi mầm non Sen vàng\' của bạn.',_binary '','{\"contractId\":24}','2025-05-14 06:19:27','2025-05-16 15:40:23',NULL,58,_binary '',4,NULL,2),(157,62,'Vui lòng đánh giá sự kiện \'Sự kiện Tết thiếu nhi mầm non Sen vàng\' của bạn.',_binary '\0','{\"contractId\":24}','2025-05-14 06:19:27','2025-05-14 06:19:27',NULL,NULL,_binary '',4,NULL,2),(158,58,'Vui lòng đánh giá sự kiện \'Sự kiện Tết thiếu nhi mầm non Sen vàng\' của bạn.',_binary '','{\"contractId\":24}','2025-05-14 06:21:29','2025-05-14 07:05:24',NULL,58,_binary '',4,NULL,0),(159,62,'Vui lòng đánh giá sự kiện \'Sự kiện Tết thiếu nhi mầm non Sen vàng\' của bạn.',_binary '','{\"contractId\":24}','2025-05-14 06:21:29','2025-05-14 07:04:49',NULL,62,_binary '',4,NULL,0);
/*!40000 ALTER TABLE `notification` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `post`
--

DROP TABLE IF EXISTS `post`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `post` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL COMMENT 'id người đăng bài',
  `caption` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `place` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'địa điểm chương trình',
  `post_group` tinyint DEFAULT NULL COMMENT 'nhóm đăng bài: 0: nhóm mc chung; 1: nhóm mc mới',
  `event_name` tinytext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'thông tin chương trình: tên ct,..',
  `event_start` datetime DEFAULT NULL COMMENT 'thời gian chương trình bắt đầu',
  `event_end` datetime DEFAULT NULL COMMENT 'thời gian chương trình kết thúc',
  `price_from` decimal(15,2) DEFAULT NULL COMMENT 'mức cát xê thấp nhất',
  `price_to` decimal(15,2) DEFAULT NULL COMMENT 'mức cát xê cao nhất',
  `mc_requirement` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'mô tả về yêu cầu dành cho mc',
  `detail_desc` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `post_client` (`user_id`),
  CONSTRAINT `post_client` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='bài post mc tìm mc dẫn chương trình cho 1 chương trình nào đó';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `post`
--

LOCK TABLES `post` WRITE;
/*!40000 ALTER TABLE `post` DISABLE KEYS */;
INSERT INTO `post` VALUES (23,62,'Tìm MC cho lễ trao bằng tốt nghiệp trường Đại học Công nghiệp Hà Nội','Trung tâm Hội nghị Quốc gia',0,'Lễ trao bằng tốt nghiệp trường Đại học Công nghiệp Hà Nội','2025-08-01 07:30:00','2025-08-01 10:30:00',43222211203.00,NULL,'Trên 5 năm kinh nghiệm',NULL,'2025-05-18 07:30:16','2025-05-18 07:52:53',62,62,_binary ''),(24,62,'Tìm MC sự kiện chuyên nghiệp','Trường THPT Số 2 TP Lào Cai',0,'Sự kiện nhà giáo VN','2025-11-20 02:00:29','2025-11-20 05:00:29',0.00,0.00,'Trẻ, năng lượng',NULL,'2025-05-18 07:56:22','2025-05-18 07:56:22',62,NULL,_binary '');
/*!40000 ALTER TABLE `post` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `province`
--

DROP TABLE IF EXISTS `province`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `province` (
  `id` int NOT NULL AUTO_INCREMENT,
  `code` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `full_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `full_name_en` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `code_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sort_order` int NOT NULL DEFAULT '0',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `idx_full_name` (`full_name`),
  KEY `idx_sort_order` (`sort_order`),
  KEY `idx_sort_order_full_name` (`sort_order`,`full_name`)
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `province`
--

LOCK TABLES `province` WRITE;
/*!40000 ALTER TABLE `province` DISABLE KEYS */;
INSERT INTO `province` VALUES (1,'01','Hà Nội','Thành phố Hà Nội','Ha Noi City','ha_noi',66,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(2,'79','Hồ Chí Minh','Thành phố Hồ Chí Minh','Ho Chi Minh City','ho_chi_minh',65,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(3,'48','Đà Nẵng','Thành phố Đà Nẵng','Da Nang City','da_nang',64,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(4,'31','Hải Phòng','Thành phố Hải Phòng','Hai Phong City','hai_phong',63,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(5,'92','Cần Thơ','Thành phố Cần Thơ','Can Tho City','can_tho',62,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(6,'02','Hà Giang','Tỉnh Hà Giang','Ha Giang Province','ha_giang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(7,'04','Cao Bằng','Tỉnh Cao Bằng','Cao Bang Province','cao_bang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(8,'06','Bắc Kạn','Tỉnh Bắc Kạn','Bac Kan Province','bac_kan',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(9,'08','Tuyên Quang','Tỉnh Tuyên Quang','Tuyen Quang Province','tuyen_quang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(10,'10','Lào Cai','Tỉnh Lào Cai','Lao Cai Province','lao_cai',61,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(11,'11','Điện Biên','Tỉnh Điện Biên','Dien Bien Province','dien_bien',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(12,'12','Lai Châu','Tỉnh Lai Châu','Lai Chau Province','lai_chau',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(13,'14','Sơn La','Tỉnh Sơn La','Son La Province','son_la',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(14,'15','Yên Bái','Tỉnh Yên Bái','Yen Bai Province','yen_bai',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(15,'17','Hoà Bình','Tỉnh Hoà Bình','Hoa Binh Province','hoa_binh',56,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(16,'19','Thái Nguyên','Tỉnh Thái Nguyên','Thai Nguyen Province','thai_nguyen',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(17,'20','Lạng Sơn','Tỉnh Lạng Sơn','Lang Son Province','lang_son',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(18,'22','Quảng Ninh','Tỉnh Quảng Ninh','Quang Ninh Province','quang_ninh',57,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(19,'24','Bắc Giang','Tỉnh Bắc Giang','Bac Giang Province','bac_giang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(20,'25','Phú Thọ','Tỉnh Phú Thọ','Phu Tho Province','phu_tho',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(21,'26','Vĩnh Phúc','Tỉnh Vĩnh Phúc','Vinh Phuc Province','vinh_phuc',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(22,'27','Bắc Ninh','Tỉnh Bắc Ninh','Bac Ninh Province','bac_ninh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(23,'30','Hải Dương','Tỉnh Hải Dương','Hai Duong Province','hai_duong',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(24,'33','Hưng Yên','Tỉnh Hưng Yên','Hung Yen Province','hung_yen',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(25,'34','Thái Bình','Tỉnh Thái Bình','Thai Binh Province','thai_binh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(26,'35','Hà Nam','Tỉnh Hà Nam','Ha Nam Province','ha_nam',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(27,'36','Nam Định','Tỉnh Nam Định','Nam Dinh Province','nam_dinh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(28,'37','Ninh Bình','Tỉnh Ninh Bình','Ninh Binh Province','ninh_binh',55,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(29,'38','Thanh Hóa','Tỉnh Thanh Hóa','Thanh Hoa Province','thanh_hoa',58,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(30,'40','Nghệ An','Tỉnh Nghệ An','Nghe An Province','nghe_an',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(31,'42','Hà Tĩnh','Tỉnh Hà Tĩnh','Ha Tinh Province','ha_tinh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(32,'44','Quảng Bình','Tỉnh Quảng Bình','Quang Binh Province','quang_binh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(33,'45','Quảng Trị','Tỉnh Quảng Trị','Quang Tri Province','quang_tri',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(34,'46','Thừa Thiên Huế','Tỉnh Thừa Thiên Huế','Thua Thien Hue Province','thua_thien_hue',59,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(35,'49','Quảng Nam','Tỉnh Quảng Nam','Quang Nam Province','quang_nam',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(36,'51','Quảng Ngãi','Tỉnh Quảng Ngãi','Quang Ngai Province','quang_ngai',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(37,'52','Bình Định','Tỉnh Bình Định','Binh Dinh Province','binh_dinh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(38,'54','Phú Yên','Tỉnh Phú Yên','Phu Yen Province','phu_yen',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(39,'56','Khánh Hòa','Tỉnh Khánh Hòa','Khanh Hoa Province','khanh_hoa',60,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(40,'58','Ninh Thuận','Tỉnh Ninh Thuận','Ninh Thuan Province','ninh_thuan',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(41,'60','Bình Thuận','Tỉnh Bình Thuận','Binh Thuan Province','binh_thuan',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(42,'62','Kon Tum','Tỉnh Kon Tum','Kon Tum Province','kon_tum',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(43,'64','Gia Lai','Tỉnh Gia Lai','Gia Lai Province','gia_lai',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(44,'66','Đắk Lắk','Tỉnh Đắk Lắk','Dak Lak Province','dak_lak',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(45,'67','Đắk Nông','Tỉnh Đắk Nông','Dak Nong Province','dak_nong',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(46,'68','Lâm Đồng','Tỉnh Lâm Đồng','Lam Dong Province','lam_dong',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(47,'70','Bình Phước','Tỉnh Bình Phước','Binh Phuoc Province','binh_phuoc',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(48,'72','Tây Ninh','Tỉnh Tây Ninh','Tay Ninh Province','tay_ninh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(49,'74','Bình Dương','Tỉnh Bình Dương','Binh Duong Province','binh_duong',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(50,'75','Đồng Nai','Tỉnh Đồng Nai','Dong Nai Province','dong_nai',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(51,'77','Bà Rịa - Vũng Tàu','Tỉnh Bà Rịa - Vũng Tàu','Ba Ria - Vung Tau Province','ba_ria_vung_tau',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(52,'80','Long An','Tỉnh Long An','Long An Province','long_an',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(53,'82','Tiền Giang','Tỉnh Tiền Giang','Tien Giang Province','tien_giang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(54,'83','Bến Tre','Tỉnh Bến Tre','Ben Tre Province','ben_tre',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(55,'84','Trà Vinh','Tỉnh Trà Vinh','Tra Vinh Province','tra_vinh',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(56,'86','Vĩnh Long','Tỉnh Vĩnh Long','Vinh Long Province','vinh_long',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(57,'87','Đồng Tháp','Tỉnh Đồng Tháp','Dong Thap Province','dong_thap',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(58,'89','An Giang','Tỉnh An Giang','An Giang Province','an_giang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(59,'91','Kiên Giang','Tỉnh Kiên Giang','Kien Giang Province','kien_giang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(60,'93','Hậu Giang','Tỉnh Hậu Giang','Hau Giang Province','hau_giang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(61,'94','Sóc Trăng','Tỉnh Sóc Trăng','Soc Trang Province','soc_trang',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(62,'95','Bạc Liêu','Tỉnh Bạc Liêu','Bac Lieu Province','bac_lieu',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary ''),(63,'96','Cà Mau','Tỉnh Cà Mau','Ca Mau Province','ca_mau',0,'2024-12-31 15:31:00','2024-12-31 15:31:00',1,1,_binary '');
/*!40000 ALTER TABLE `province` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reaction`
--

DROP TABLE IF EXISTS `reaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reaction` (
  `id` int NOT NULL AUTO_INCREMENT,
  `post_id` int NOT NULL,
  `user_id` int DEFAULT NULL,
  `user_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `type` tinyint NOT NULL DEFAULT '0' COMMENT '0: like; 1: tim:',
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `reaction_post_id_idx` (`post_id`),
  CONSTRAINT `reaction_post_id` FOREIGN KEY (`post_id`) REFERENCES `post` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=80 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='lưu reaction như like, tim của người dùng vào các bài post tìm MC';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reaction`
--

LOCK TABLES `reaction` WRITE;
/*!40000 ALTER TABLE `reaction` DISABLE KEYS */;
INSERT INTO `reaction` VALUES (69,24,62,'Hoàng Văn Khách',0,'2025-05-18 15:43:41','2025-05-18 15:43:41',62,NULL,_binary ''),(71,23,62,'Hoàng Văn Khách',0,'2025-05-18 15:44:36','2025-05-18 15:44:36',62,NULL,_binary ''),(78,24,58,'Công Trịnh Quý',0,'2025-05-18 15:51:18','2025-05-18 15:51:18',58,NULL,_binary ''),(79,23,58,'Công Trịnh Quý',0,'2025-05-18 15:51:19','2025-05-18 15:51:19',58,NULL,_binary '');
/*!40000 ALTER TABLE `reaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `resource`
--

DROP TABLE IF EXISTS `resource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `resource` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`),
  KEY `idx_resource_name` (`name`),
  KEY `idx_resource_description` (`description`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `resource`
--

LOCK TABLES `resource` WRITE;
/*!40000 ALTER TABLE `resource` DISABLE KEYS */;
/*!40000 ALTER TABLE `resource` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `resource_permission`
--

DROP TABLE IF EXISTS `resource_permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `resource_permission` (
  `id` int NOT NULL AUTO_INCREMENT,
  `resource_id` int NOT NULL,
  `permission` enum('READ','CREATE','UPDATE','DELETE') NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uniq_resource_permission` (`resource_id`,`permission`),
  CONSTRAINT `resource_permission_ibfk_1` FOREIGN KEY (`resource_id`) REFERENCES `resource` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `resource_permission`
--

LOCK TABLES `resource_permission` WRITE;
/*!40000 ALTER TABLE `resource_permission` DISABLE KEYS */;
/*!40000 ALTER TABLE `resource_permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`),
  KEY `idx_role_name` (`name`),
  KEY `idx_role_description` (`description`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role_resource_permission`
--

DROP TABLE IF EXISTS `role_resource_permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_resource_permission` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role_id` int NOT NULL,
  `resource_permission_id` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `role_resource_permission_ibfk_1` (`role_id`),
  KEY `role_resource_permission_ibfk_2` (`resource_permission_id`),
  CONSTRAINT `role_resource_permission_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE CASCADE,
  CONSTRAINT `role_resource_permission_ibfk_2` FOREIGN KEY (`resource_permission_id`) REFERENCES `resource_permission` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role_resource_permission`
--

LOCK TABLES `role_resource_permission` WRITE;
/*!40000 ALTER TABLE `role_resource_permission` DISABLE KEYS */;
/*!40000 ALTER TABLE `role_resource_permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `full_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Họ tên lấy từ tk google',
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Email lấy từ tk google',
  `phone_number` varchar(12) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'Số điện thoại lấy từ tk google',
  `is_mc` bit(1) DEFAULT b'0' COMMENT 'người dùng có phải MC hay ko',
  `age` int DEFAULT NULL COMMENT 'tuổi',
  `nick_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'nghệ danh của MC',
  `credit` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT 'điểm uy tín của MC',
  `gender` tinyint DEFAULT '0' COMMENT 'giới tính: 0 - chưa set; 1 - nam; 2 - nữ; 3 - khác',
  `is_newbie` bit(1) NOT NULL DEFAULT b'0' COMMENT 'MC có phải MC mới ko',
  `working_area` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'khu vực MC hoạt động',
  `is_verified` bit(1) NOT NULL DEFAULT b'0' COMMENT 'MC đã được xác minh danh tính chưa',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'Mô tả về MC',
  `education` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'Học vấn MC',
  `height` decimal(4,1) DEFAULT NULL COMMENT 'chiều cao',
  `weight` decimal(4,1) DEFAULT NULL COMMENT 'cân nặng',
  `avatar_url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'link để lấy avatar dịch vụ lưu ảnh',
  `facebook` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci COMMENT 'link facebook',
  `zalo` varchar(12) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT 'số liên hệ zalo',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `idx_user_email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='người dùng. nếu is_mc=1 thì người dùng vừa là mc, vừa là khách tìm mc, nếu is_mc=0 thì người dùng là khách tìm mc';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (58,'Công Trịnh Quý','trinhquycong@gmail.com',NULL,_binary '',24,'Công Trịnh Quý',100.00,1,_binary '',NULL,_binary '','Vui vẻ, tận tâm với nghề MC','Đại học',168.0,56.0,'https://mc-booking-platform.s3.amazonaws.com/images/f301e3b8-1ff5-4908-bf51-109b270a7a34-fb-avt.jpg','https://www.facebook.com/profile.php?id=100001609519740&locale=vi_VN','0333666999','2025-01-12 22:22:37','2025-05-18 04:51:13',NULL,58,_binary ''),(62,'Hoàng Văn Khách','congtrinhh.vlogger@gmail.com',NULL,_binary '\0',20,'Cong Trinh',10.00,0,_binary '\0',NULL,_binary '\0',NULL,NULL,NULL,NULL,'https://lh3.googleusercontent.com/a/ACg8ocLqX0CMmJm89k2nThAYWG4S6RrQrsIEAzxznlzmND5yva4MWw=s96-c',NULL,NULL,'2025-03-27 15:15:56','2025-05-18 08:39:43',NULL,62,_binary ''),(67,'M606 Haui','lab6m606@gmail.com',NULL,_binary '',20,'Thanh Thủy',10.00,2,_binary '\0',NULL,_binary '\0','Em là dân Hải Phòng nên em không lòng vòng, hãy book em đi','Đại học',NULL,NULL,'https://mc-booking-platform.s3.amazonaws.com/images/ceeb6024-fb5b-4553-ab82-f2c58127ff89-Thanh Thủy.png',NULL,NULL,'2025-05-18 04:01:28','2025-05-18 04:06:20',NULL,67,_binary ''),(68,'Trinh Quy Cong','trinhquycong.ielts@gmail.com',NULL,_binary '',30,'Ly Ly',10.00,2,_binary '\0',NULL,_binary '\0','Trúc xinh trúc đứng một mình.\nEm xinh em đứng một mình nhận show','Đại học',NULL,NULL,'https://mc-booking-platform.s3.amazonaws.com/images/81a0e360-1928-4c8a-8ef4-4bd892b3b2e9-Ly Ly.png',NULL,NULL,'2025-05-18 04:07:37','2025-05-18 04:09:33',NULL,68,_binary ''),(69,'Cong Trinh','0love0problem@gmail.com',NULL,_binary '',30,'Hoàng Giang',10.00,1,_binary '\0',NULL,_binary '\0',NULL,'Đại học',NULL,NULL,'https://mc-booking-platform.s3.amazonaws.com/images/c0da547d-6315-4a9d-93e1-c2b0f654f3c1-Hoàng Giang.png',NULL,NULL,'2025-05-18 04:17:51','2025-05-18 04:21:48',NULL,69,_binary '');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_id_verification`
--

DROP TABLE IF EXISTS `user_id_verification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_id_verification` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'ID của bản ghi xác minh danh tính',
  `user_id` int NOT NULL COMMENT 'ID người dùng, liên kết với bảng user',
  `face_image_url` varchar(512) DEFAULT NULL COMMENT 'Đường dẫn URL ảnh khuôn mặt của người dùng (Bước 1)',
  `id_front_image_url` varchar(512) DEFAULT NULL COMMENT 'Đường dẫn URL ảnh mặt trước của CCCD (Bước 2)',
  `id_back_image_url` varchar(512) DEFAULT NULL COMMENT 'Đường dẫn URL ảnh mặt sau của CCCD (Bước 2)',
  `current_step` tinyint DEFAULT '0' COMMENT 'Bước hiện tại của quá trình xác minh: 0 - face (ảnh khuôn mặt),  1 - id_front (ảnh mặt trước), 2 - id_back (ảnh mặt sau), 3 - claim (bước xác nhận thông tin)',
  `status` tinyint DEFAULT '0' COMMENT 'Trạng thái xác minh: 0 - pending (đang chờ), 1 - verified (đã xác minh), 2 - rejected (bị từ chối)',
  `verified_at` timestamp NULL DEFAULT NULL COMMENT 'Thời gian xác minh thành công (null nếu chưa xác minh)',
  `error_message` text COMMENT 'Thông báo lỗi nếu quá trình xác minh thất bại',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` int DEFAULT NULL,
  `modified_by` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT b'1' COMMENT 'bản ghi này có đang được sử dụng ko. nếu ko đc sử dụng thì ko lấy lên giao diện',
  PRIMARY KEY (`id`),
  KEY `user_id_verification_ibfk_1` (`user_id`),
  CONSTRAINT `user_id_verification_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Bảng lưu trữ quá trình xác minh danh tính của người dùng, bao gồm ảnh và tình trạng xử lý';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_id_verification`
--

LOCK TABLES `user_id_verification` WRITE;
/*!40000 ALTER TABLE `user_id_verification` DISABLE KEYS */;
INSERT INTO `user_id_verification` VALUES (13,58,'https://mc-booking-platform.s3.amazonaws.com/identity-verification/58/face_638828028266926556.jpg','https://mc-booking-platform.s3.amazonaws.com/identity-verification/58/id_front_638828028442020065.jpg','https://mc-booking-platform.s3.amazonaws.com/identity-verification/58/id_back_638828028758988807.jpg',3,1,'2025-05-14 00:01:40',NULL,'2025-05-14 06:59:29','2025-05-14 07:01:39',NULL,NULL,_binary '');
/*!40000 ALTER TABLE `user_id_verification` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'mbp'
--
/*!50003 DROP PROCEDURE IF EXISTS `proc_client_review_mc_get_paged` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`admin`@`%` PROCEDURE `proc_client_review_mc_get_paged`(
    $McId INT, 
    $IsGetContract BIT,
    $IsGetMc BIT,
    $IsGetClient BIT,
    $OrderBy VARCHAR(255),
    $Limit INT,
    $Offset INT
)
    SQL SECURITY INVOKER
BEGIN
    DROP TEMPORARY TABLE IF EXISTS tmp_client_review_mc;
    SET @sql = 'CREATE TEMPORARY TABLE tmp_client_review_mc AS ( SELECT cr.* FROM client_review_mc cr WHERE 1=1';
    
    IF $McId IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND cr.mc_id = ', $McId);
    END IF;
 
    SET @sql = CONCAT(@sql, ' )');
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    
    DROP TEMPORARY TABLE IF EXISTS tmp_client_review_mc_paged;
    SET @sqlPaged = 'CREATE TEMPORARY TABLE tmp_client_review_mc_paged AS SELECT * FROM tmp_client_review_mc';
    
    IF $OrderBy IS NOT NULL  AND $OrderBy <> '' THEN
        SET @sqlPaged = CONCAT(@sqlPaged, ' ORDER BY ', $OrderBy);
    END IF;
    
    IF $Limit IS NOT NULL  AND $Limit <> '' THEN
        SET @sqlPaged = CONCAT(@sqlPaged, ' LIMIT ', $Limit);
    END IF;
    
    IF $Offset IS NOT NULL AND $Offset <> ''  THEN
        SET @sqlPaged = CONCAT(@sqlPaged, ' OFFSET ', $Offset);
    END IF;
    
    PREPARE stmtPaged FROM @sqlPaged;
    EXECUTE stmtPaged;
    DEALLOCATE PREPARE stmtPaged;
    
    SELECT * FROM tmp_client_review_mc_paged;
    SELECT COUNT(1) FROM tmp_client_review_mc;

    IF $IsGetContract THEN
        SELECT c.*
        FROM tmp_client_review_mc_paged cr
        INNER JOIN contract c ON cr.contract_id = c.id;
    END IF;
    IF $IsGetMc THEN
        SELECT u.*
        FROM tmp_client_review_mc_paged cr
        INNER JOIN user u ON cr.mc_id = u.id;
    END IF;
    IF $IsGetClient THEN
        SELECT u.*
        FROM tmp_client_review_mc_paged cr
        INNER JOIN user u ON cr.client_id = u.id;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `proc_contract_get_by_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_contract_get_by_id`(IN $id INT)
    SQL SECURITY INVOKER
BEGIN
	-- contract
    SELECT * FROM contract WHERE id = $id;
    
    -- mc
    SELECT u.* 
      FROM user u
      JOIN contract c ON c.mc_id = u.id
     WHERE c.id = $id
     group by u.id
     limit 1;
     
	-- client
    SELECT u.* 
      FROM user u
      JOIN contract c ON c.client_id = u.id
     WHERE c.id = $id
     group by u.id
     limit 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `proc_contract_get_paged` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`admin`@`%` PROCEDURE `proc_contract_get_paged`(
    IN $ClientId INT,
    IN $McId INT,
    IN $KeyWord VARCHAR(255),
    IN $Status INT,
    IN $OrderBy VARCHAR(255),
    IN $Limit INT,
    IN $Offset INT
)
    SQL SECURITY INVOKER
BEGIN
    -- create temporary table to store all contracts that meet the filter conditions (before paging)
    DROP TEMPORARY TABLE IF EXISTS tmp_contract;
    SET @sql = 'CREATE TEMPORARY TABLE tmp_contract AS ( SELECT c.* FROM contract c';
    
    SET @sql = CONCAT(@sql, ' WHERE 1=1');
    
    IF $KeyWord IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND c.description LIKE ''%', $KeyWord, '%''');
    END IF;
    
    IF $ClientId IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND c.client_id = ', $ClientId);
    END IF;
    
    IF $McId IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND c.mc_id = ', $McId);
    END IF;
    
    IF $Status IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND c.status = ', $Status);
    END IF;
        
    SET @sql = CONCAT(@sql, ' )');
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    
    -- create temporary table for paged contracts
    DROP TEMPORARY TABLE IF EXISTS tmp_contract_paged;
    SET @sqlContractPaged = concat('CREATE TEMPORARY TABLE tmp_contract_paged AS SELECT * FROM tmp_contract');
    
    IF $OrderBy IS NOT NULL THEN
        SET @sqlContractPaged = CONCAT(@sqlContractPaged, ' ORDER BY ', $OrderBy);
    END IF;

    IF $Limit IS NOT NULL THEN
        SET @sqlContractPaged = CONCAT(@sqlContractPaged, ' LIMIT ', $Limit);
    END IF;

    IF $Offset IS NOT NULL THEN
        SET @sqlContractPaged = CONCAT(@sqlContractPaged, ' OFFSET ', $Offset);
    END IF;

    PREPARE stmtContractPaged FROM @sqlContractPaged;
    EXECUTE stmtContractPaged;
    DEALLOCATE PREPARE stmtContractPaged;

    -- paged data
    SELECT * FROM tmp_contract_paged;

    -- total count
    SELECT COUNT(1) FROM tmp_contract;
    
    -- mcs
    SELECT u.* FROM user u INNER JOIN tmp_contract_paged t ON t.mc_id = u.id group by u.id;
    
    -- clients
    SELECT  u.* FROM user u INNER JOIN tmp_contract_paged t ON t.client_id = u.id group by u.id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `proc_mc_review_client_get_paged` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`admin`@`%` PROCEDURE `proc_mc_review_client_get_paged`(
    $ClientId INT, 
    $IsGetContract BIT,
    $IsGetMc BIT,
    $IsGetClient BIT,
    $OrderBy VARCHAR(255),
    $Limit INT,
    $Offset INT
)
    SQL SECURITY INVOKER
BEGIN
    DROP TEMPORARY TABLE IF EXISTS tmp_mc_review_client;
    SET @sql = 'CREATE TEMPORARY TABLE tmp_mc_review_client AS ( SELECT mr.* FROM mc_review_client mr WHERE 1=1';
    
    IF $ClientId IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND mr.client_id = ', $ClientId);
    END IF;
 
    SET @sql = CONCAT(@sql, ' )');
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    
    DROP TEMPORARY TABLE IF EXISTS tmp_mc_review_client_paged;
    SET @sqlPaged = 'CREATE TEMPORARY TABLE tmp_mc_review_client_paged AS SELECT * FROM tmp_mc_review_client';
    
    IF $OrderBy IS NOT NULL  AND $OrderBy <> '' THEN
        SET @sqlPaged = CONCAT(@sqlPaged, ' ORDER BY ', $OrderBy);
    END IF;
    
    IF $Limit IS NOT NULL  AND $Limit <> '' THEN
        SET @sqlPaged = CONCAT(@sqlPaged, ' LIMIT ', $Limit);
    END IF;
    
    IF $Offset IS NOT NULL AND $Offset <> ''  THEN
        SET @sqlPaged = CONCAT(@sqlPaged, ' OFFSET ', $Offset);
    END IF;
    
    PREPARE stmtPaged FROM @sqlPaged;
    EXECUTE stmtPaged;
    DEALLOCATE PREPARE stmtPaged;
    
    SELECT * FROM tmp_mc_review_client_paged;
    SELECT COUNT(1) FROM tmp_mc_review_client;

    IF $IsGetContract THEN
        SELECT c.*
        FROM tmp_mc_review_client_paged mr
        INNER JOIN contract c ON mr.contract_id = c.id;
    END IF;
    IF $IsGetMc THEN
        SELECT u.*
        FROM tmp_mc_review_client_paged mr
        INNER JOIN user u ON mr.mc_id = u.id;
    END IF;
    IF $IsGetClient THEN
        SELECT u.*
        FROM tmp_mc_review_client_paged mr
        INNER JOIN user u ON mr.client_id = u.id;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `proc_post_get_paged` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_post_get_paged`(
IN $PostGroup INT,
    IN $KeyWord VARCHAR(255),
    IN $OrderBy VARCHAR(255),
    IN $IsGetReaction bit,
    IN $Limit INT,
    IN $Offset INT
)
BEGIN
   -- tạo bảng tạm lưu tất cả user thỏa mãn điều kiện lọc (chưa paging)
    DROP TEMPORARY TABLE IF EXISTS tmp_post;
    SET @sql = 'CREATE TEMPORARY TABLE tmp_post AS ( SELECT u.* FROM post u';
    
    SET @sql = CONCAT(@sql, ' WHERE 1=1');
    
    IF $KeyWord IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.caption LIKE ''%', $KeyWord, '%''');
    END IF;
    
    IF $PostGroup IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.post_group = ', $PostGroup);
    END IF;
        
    SET @sql = CONCAT(@sql, ' )');
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    
    -- tạo bảng user đã paging
    DROP TEMPORARY TABLE IF EXISTS tmp_post_paged;
    SET @sqlUserPaged = concat('CREATE TEMPORARY TABLE tmp_post_paged AS SELECT * FROM tmp_post');
    
    IF $OrderBy IS NOT NULL THEN
    SET @sqlUserPaged = CONCAT(@sqlUserPaged, ' ORDER BY ', $OrderBy);
END IF;

IF $Limit IS NOT NULL THEN
    SET @sqlUserPaged = CONCAT(@sqlUserPaged, ' LIMIT ', $Limit);
END IF;

IF $Offset IS NOT NULL THEN
    SET @sqlUserPaged = CONCAT(@sqlUserPaged, ' OFFSET ', $Offset);
END IF;

PREPARE stmtUserPaged FROM @sqlUserPaged;
EXECUTE stmtUserPaged;
DEALLOCATE PREPARE stmtUserPaged;

-- paged data
SELECT * FROM tmp_post_paged;

-- total count
SELECT COUNT(1) FROM tmp_post;

-- user (mapping 1 - 1 )
select u.* from user u inner join tmp_post_paged t on t.user_id = u.id
group by u.id;

-- details: reaction
IF $IsGetReaction THEN
    SELECT r.* from reaction r INNER JOIN tmp_post_paged t on t.id = r.post_id;
END IF; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `proc_user_get_by_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`admin`@`%` PROCEDURE `proc_user_get_by_id`($id INT)
    SQL SECURITY INVOKER
BEGIN
    -- First result: user info
    SELECT *
    FROM `user`
    WHERE id = $id;

    -- Second result: hosting styles
    SELECT hs.*
    FROM mc_hosting_style mhs
    INNER JOIN hosting_style hs ON hs.id = mhs.hosting_style_id
    WHERE mhs.mc_id = $id;

    -- Third result: mc types
    SELECT mt.*
    FROM mc_mc_type mmt
    INNER JOIN mc_type mt ON mt.id = mmt.mc_type_id
    WHERE mmt.mc_id = $id;

    -- Fourth result: provinces
    SELECT p.*
    FROM mc_province mp
    INNER JOIN province p ON p.id = mp.province_id
    WHERE mp.mc_id = $id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `proc_user_get_paged` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_user_get_paged`(
    $FullName VARCHAR(255), 
    $Email VARCHAR(255), 
    $PhoneNumber VARCHAR(12), 
    $IsMc BIT, 
    $IsVerified BIT, 
    $IsNewbie BIT, 
    $NickName VARCHAR(255),  
    $MinAge INT, 
    $MaxAge INT, 
    $IsGetMedia BIT, 
    $IsGetMcType BIT, 
    $IsGetProvince BIT,
    $McTypeIds TEXT, 
    $HostingStyleIds TEXT, 
    $ProvinceIds TEXT, 
    $Genders VARCHAR(255),
    $OrderBy VARCHAR(255),
    $Limit INT,
    $Offset INT
)
BEGIN
	-- tạo bảng tạm lưu tất cả user thỏa mãn điều kiện lọc (chưa paging)
    DROP TEMPORARY TABLE IF EXISTS tmp_user;
    SET @sql = 'CREATE TEMPORARY TABLE tmp_user AS ( SELECT u.* FROM user u';
    
    IF $McTypeIds IS NOT NULL AND $McTypeIds <> '' THEN
        SET @sql = CONCAT(@sql, ' INNER JOIN mc_mc_type mmt ON mmt.mc_id = u.id');
    END IF;
    
    IF $HostingStyleIds IS NOT NULL AND $HostingStyleIds <> '' THEN
        SET @sql = CONCAT(@sql, ' INNER JOIN mc_hosting_style mhs ON mhs.mc_id = u.id');
    END IF;
    
    IF $ProvinceIds IS NOT NULL AND $ProvinceIds <> '' THEN
        SET @sql = CONCAT(@sql, ' INNER JOIN mc_province mp ON mp.mc_id = u.id');
    END IF;
    
    SET @sql = CONCAT(@sql, ' WHERE 1=1');
    
    IF $FullName IS NOT NULL AND $FullName <> ''  THEN
        SET @sql = CONCAT(@sql, ' AND u.full_name LIKE ''%', $FullName, '%''');
    END IF;
    
    IF $Email IS NOT NULL AND $Email <> '' THEN
        SET @sql = CONCAT(@sql, ' AND u.email LIKE ''%', $Email, '%''');
    END IF;
    
    IF $PhoneNumber IS NOT NULL AND $PhoneNumber <> '' THEN
        SET @sql = CONCAT(@sql, ' AND u.phone_number LIKE ''%', $PhoneNumber, '%''');
    END IF;
    
    IF $IsMc IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.is_mc = ', IF ($IsMc, 1, 0));
    END IF;
    
    IF $IsVerified IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.is_verified = ', IF ($IsVerified, 1, 0));
    END IF;
    
    IF $IsNewbie IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.is_newbie = ',  IF ($IsNewbie, 1, 0));
    END IF;
    
    IF $NickName IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.nick_name LIKE ''%', $NickName, '%''');
    END IF;
    
    IF $MinAge IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.age >= ', $MinAge);
    END IF;
    
    IF $MaxAge IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND u.age <= ', $MaxAge);
    END IF; 
    
    IF $McTypeIds IS NOT NULL and $McTypeIds <> '' THEN
        SET @sql = CONCAT(@sql, ' AND mmt.mc_type_id IN (', $McTypeIds, ')');
    END IF;
    
    IF $HostingStyleIds IS NOT NULL and $HostingStyleIds <> '' THEN
        SET @sql = CONCAT(@sql, ' AND mhs.hosting_style_id IN (', $HostingStyleIds, ')');
    END IF;
    
    IF $ProvinceIds IS NOT NULL and $ProvinceIds <> '' THEN
        SET @sql = CONCAT(@sql, ' AND mp.province_id IN (', $ProvinceIds, ')');
    END IF;
    
    IF $Genders IS NOT NULL and $Genders <> '' THEN
        SET @sql = CONCAT(@sql, ' AND u.gender IN (', $Genders, ')');
    END IF;
    
    SET @sql = CONCAT(@sql, ' GROUP BY u.id');
        
    SET @sql = CONCAT(@sql, ' )');
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    
    -- tạo bảng user đã paging
    DROP TEMPORARY TABLE IF EXISTS tmp_user_paged;
    SET @sqlUserPaged = concat('CREATE TEMPORARY TABLE tmp_user_paged AS SELECT * FROM tmp_user');
    
    IF $OrderBy IS NOT NULL THEN
    SET @sqlUserPaged = CONCAT(@sqlUserPaged, ' ORDER BY ', $OrderBy);
END IF;

IF $Limit IS NOT NULL THEN
    SET @sqlUserPaged = CONCAT(@sqlUserPaged, ' LIMIT ', $Limit);
END IF;

IF $Offset IS NOT NULL THEN
    SET @sqlUserPaged = CONCAT(@sqlUserPaged, ' OFFSET ', $Offset);
END IF;

PREPARE stmtUserPaged FROM @sqlUserPaged;
EXECUTE stmtUserPaged;
DEALLOCATE PREPARE stmtUserPaged;

-- paged data
SELECT * FROM tmp_user_paged;

-- total count
SELECT COUNT(1) FROM tmp_user;

-- details: join detail with paged user table to get details by user
IF $IsGetMcType THEN
    SELECT u.id AS mc_id, mt.*
    FROM tmp_user_paged u
    INNER JOIN mc_mc_type mmt ON mmt.mc_id = u.id
    INNER JOIN mc_type mt ON mmt.mc_type_id = mt.id;
END IF;

IF $IsGetProvince THEN
    SELECT u.id AS user_id, p.*
    FROM tmp_user_paged u
    INNER JOIN mc_province mp ON mp.mc_id = u.id
    INNER JOIN province p ON mp.province_id= p.id;
END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-18 18:33:06
