-- ----------------------------
-- Table structure for eventportfunction
-- ----------------------------
DROP TABLE IF EXISTS `eventportfunction`;
CREATE TABLE `eventportfunction` (
  `id` int(2) NOT NULL,
  `username` varchar(255) NOT NULL,
  `room` varchar(255) NOT NULL,
  `event` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of eventportfunction
-- ----------------------------
