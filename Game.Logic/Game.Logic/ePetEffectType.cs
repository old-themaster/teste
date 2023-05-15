namespace Game.Logic
{
    public enum ePetEffectType
    {
        PetAddAgilityEquip = 1,
        PetAddAttackEquip = 2,
        PetAddMaxBloodEquipEffect = 3,
        AddDamageEffect = 4,
        AddDefenceEquip = 5,
        PetAddLuckEquip = 6,
        FatalEffect = 7,
        IceFronzeEffect = 9,
        IceFronzeEquipEffect = 10, // 0x0000000A
        NoHoleEffect = 12, // 0x0000000C
        NoHoleEquipEffect = 13, // 0x0000000D
        ReduceDamageEffect = 14, // 0x0000000E
        SealEffect = 15, // 0x0000000F
        AtomBomb = 16, // 0x00000010
        ArmorPiercer = 17, // 0x00000011
        SealEquipEffect = 18, // 0x00000012
        AddTurnEquipEffect = 19, // 0x00000013
        AddDander = 20, // 0x00000014
        ReflexDamageEquipEffect = 21, // 0x00000015
        ReduceStrengthEffect = 22, // 0x00000016
        ContinueDamageEffect = 23, // 0x00000017
        AddBombEquipEffect = 24, // 0x00000018
        AvoidDamageEffect = 25, // 0x00000019
        MakeCriticalEffect = 26, // 0x0000001A
        AssimilateDamageEffect = 27, // 0x0000001B
        AssimilateBloodEffect = 28, // 0x0000001C
        ReflexDamageEffect = 29, // 0x0000001D
        ContinueReduceBaseDamageEquipEffect = 30, // 0x0000001E
        ReduceStrengthEquipEffect = 31, // 0x0000001F
        ContinueReduceBloodEquipEffect = 32, // 0x00000020
        ContinueReduceBloodEffect = 33, // 0x00000021
        LockDirectionEquipEffect = 34, // 0x00000022
        LockDirectionEffect = 35, // 0x00000023
        ContinueReduceBaseDamageEffect = 36, // 0x00000024
        RecoverBloodEffect = 37, // 0x00000025
        ContinueReduceDamageEffect = 38, // 0x00000026
        PetReduceAttackEquip = 39, // 0x00000027
        PetReduceAttackEquipEffect = 40, // 0x00000028
        PetDefendEquipEffect = 41, // 0x00000029
        PetStopMovingEquip = 42, // 0x0000002A
        PetStopMovingEquipEffect = 43, // 0x0000002B
        PetPlusGuardEquipEffect = 44, // 0x0000002C
        PetPlusDameEquipEffect = 45, // 0x0000002D
        PetRemovePlusDameEquipEffect = 46, // 0x0000002E
        PetClearPlusGuardEquipEffect = 47, // 0x0000002F
        PetPlusOneMpEquipEffect = 48, // 0x00000030
        PetPlusTwoMpEquipEffect = 49, // 0x00000031
        PetPlusThreeMpEquipEffect = 50, // 0x00000032
        PetAttackAroundEquipEffect = 51, // 0x00000033
        PetPlusAllTwoMpEquipEffect = 51, // 0x00000033
        PetAddAttackEquipEffect = 52, // 0x00000034
        PetAddLuckAllMatchEquipEffect = 53, // 0x00000035
        PetReduceDefendEquipEffect = 54, // 0x00000036
        PetAlwayNoHoleEquipEffect = 55, // 0x00000037
        PetClearV3BatteryEquipEffect = 56, // 0x00000038
        PetFieryAntBallPlusdefend = 57, // 0x00000039
        PetReduceTakeDamageEquipEffect = 58, // 0x0000003A
        PetReduceDamageEquipEffect = 59, // 0x0000003B
        PetReduceDamageEquip = 60, // 0x0000003C
        PetReduceTakeDamageEquip = 61, // 0x0000003D
        PetMakeDamageEquip = 62, // 0x0000003E
        PetMakeDamageEquipEffect = 63, // 0x0000003F
        PetAddDamageEquip = 64, // 0x00000040
        PetLuckMakeDamageEquipEffect = 65, // 0x00000041
        PetRemoveDamageEquip = 66, // 0x00000042
        PetRemoveDamageEquipEffect = 67, // 0x00000043
        PetAddBloodEquip = 68, // 0x00000044
        PetAddBloodEquipEffect = 69, // 0x00000045
        PetAddDamageEquipEffect = 70, // 0x00000046
        PetUnlimitAddBloodEquip = 71, // 0x00000047
        PetUnlimitAddBloodEquipEffect = 72, // 0x00000048
        PetAddBloodAllPlayerAroundEquip = 73, // 0x00000049
        PetAddBloodAllPlayerAroundEquipEffect = 74, // 0x0000004A
        PetRevertBloodAllPlayerAroundEquipEffect = 75, // 0x0000004B
        PetAddBloodForSelfEquip = 76, // 0x0000004C
        PetAddBloodForSelfEquipEffect = 77, // 0x0000004D
        PetAddBloodForTeamEquipEffect = 78, // 0x0000004E
        PetAddDefendByCureEquipEffect = 79, // 0x0000004F
        PetReduceTargetAttackEquipEffect = 80, // 0x00000050
        PetAddGuardEquip = 81, // 0x00000051
        PetAddGuardForTeamEquipEffect = 82, // 0x00000052
        PetRecoverBloodForTeamEquipEffect = 83, // 0x00000053
        PetAddBloodSelfEquip = 84, // 0x00000054
        PetRecoverMPForTeamEquipEffect = 85, // 0x00000055
        PetRemoveTagertMPEquipEffect = 86, // 0x00000056
        PetAddGodLuckEquipEffect = 87, // 0x00000057
        PetAddGodDamageEquip = 88, // 0x00000058
        PetAddGodDamageEquipEffect = 99, // 0x00000063
        PetAtomBombEquipEffect = 100, // 0x00000064
        PetReduceTargetBloodEquipEffect = 101, // 0x00000065
        PetReduceBloodEquip = 102, // 0x00000066
        PetAddLuckLimitTurnEquipEffect = 103, // 0x00000067
        PetActiveGuardForTeamEquipEffect = 104, // 0x00000068
        PetActiveGuardEquip = 105, // 0x00000069
        PetShootedAddGuardForTeamEquip = 106, // 0x0000006A
        PetActiveDamageForTeamEquipEffect = 107, // 0x0000006B
        PetShootedAddDamageForTeamEquip = 108, // 0x0000006C
        PetActiveDamageEquip = 109, // 0x0000006D
        PetRecoverBloodForTeamInMapEquipEffect = 110, // 0x0000006E
        PetSecondWeaponBonusPointEquipEffect = 111, // 0x0000006F
        PetGuardSecondWeaponRecoverBloodEquipEffect = 112, // 0x00000070
        PetAddHighMPEquipEffect = 113, // 0x00000071
        PetClearHighMPEquipEffect = 114, // 0x00000072
        PetBonusAttackForTeamEquipEffect = 115, // 0x00000073
        PetBonusDefendForTeamEquipEffect = 116, // 0x00000074
        PetBonusAgilityForTeamEquipEffect = 117, // 0x00000075
        PetBonusLuckyForTeamEquipEffect = 118, // 0x00000076
        PetBonusMaxHpForTeamEquipEffect = 119, // 0x00000077
        PetReduceBaseDamageEquip = 120, // 0x00000078
        PetReduceDefendEquip = 121, // 0x00000079
        PetReduceMpAllEnemyEquipEffect = 122, // 0x0000007A
        PetAddCritRateEquip = 123, // 0x0000007B
        PetReduceBloodAllBattleEquip = 124, // 0x0000007C
        PetReduceGuardAllEnemyEquipEffect = 125, // 0x0000007D
        PetReduceBaseGuardEquip = 126, // 0x0000007E
        PetStopMovingAllEnemyEffect = 127, // 0x0000007F
        PetBonusGuardBeginMatchEquipEffect = 128, // 0x00000080
        PetBonusMaxBloodBeginMatchEquipEffect = 129, // 0x00000081
        PetReduceBloodAllBattleEquipEffectcs = 130, // 0x00000082
        PetAddCritRateEquipEffect = 131, // 0x00000083
        PetReduceAttackAllEnemyEquipEffect = 132, // 0x00000084
        PetReduceDefendAllEnemyEquipEffect = 133, // 0x00000085
        PetReduceBaseDamageTargetEquipEffect = 134, // 0x00000086
        PetBurningBloodTargetEquipEffect = 135, // 0x00000087
        PetBonusAttackBeginMatchEquipEffect = 136, // 0x00000088
        PetBonusBaseDamageBeginMatchEquipEffect = 137, // 0x00000089
        PetReduceBaseGuardEquipEffect = 138, // 0x0000008A
        PetDamageAllEnemyEquipEffect = 139, // 0x0000008B
        PetBurningBloodShootingEquip = 140, // 0x0000008C
        PetEnemyAttackBurningBloodEquipEffect = 141, // 0x0000008D
        PetAttackedRecoverBloodEquip = 142, // 0x0000008E
        PetAttackedRecoverBloodEquipEffect = 143, // 0x0000008F
        PetBuffAttackEquipEffect = 144, // 0x00000090
        PetBuffBaseGuardForTeamEquipEffect = 145, // 0x00000091
        PetBuffGuardEquip = 146, // 0x00000092
        PetClearHellIceEquipEffectcs = 147, // 0x00000093
        PetBonusAgilityBeginMatchEquipEffect = 148, // 0x00000094
        PetBonusDefendTeamBeginMatchEquipEffect = 149, // 0x00000095
        PetBonusAttackBeginMatchEquip = 150,// 0x00000096
        DamageInCreaseLv1 = 151,
        DamageInCreaseLv2 = 152,
        DamageInCreaseLv3 = 153,
        GuideShoot = 154,
        GuideShoots = 155,
        CriticalChance = 156,
        InCreaseDefenceLv1 = 157,
        InCreaseDefenceLv2 = 158,
        InCreaseDefenceLv3 = 159,
        InCreaseDefenceLv1A = 160,
        InCreaseDefenceLv2A = 161,
        InCreaseDefenceLv3A = 162,
        RemoveAttackLv1 = 163,
        RemoveAttackLv1A = 164,
        RemoveAttackLv2 = 165,
        RemoveAttackLv2A = 166,
        ImmunityEffect = 167,
        ImmunityEffects = 168,
        PlayerStopMovingEffect = 169,
        PlayerStopMovingEffects = 170,
        AddBaseGuardLv1 = 171,
        AddBaseGuardLv1A = 172,
        AddBaseGuardLv2 = 173,
        AddBaseGuardLv2A = 174,
        RemoveEffectForAddBaseGuard = 175,
        AdditionalDamagePointLv1 = 176,
        AdditionalDamagePointLv2 = 177,
        UseBloodAndShootLv1 = 178,
        UseBloodAndShootLv2 = 179,
        CantDropPlayer = 180,
        AddBaseDamageEffectLv1 = 181,
        AddBaseDamageEffectLv2 = 182,
        AddLuckyEffectLv1 = 183,
        AddLuckyEffectLv2 = 184,
        RemoveBaseGuardEffectLv1 = 185,
        RemoveBaseGuardEffectLv2 = 186,
        RemoveEffectSkill01 = 187,
        RemoveEffectSkill02 = 188,
        AddMagicPointPet = 189,
        RemoveBloodOverTurn = 190,
        CASE1133 = 191,
        CASE1201 = 192,
        CASE1201A = 193,
        CASE1039 = 194,
        CASE1202 = 195,
        CASE1202A = 196,
        CASE1134 = 197,
        CASE1203 = 198,
        CASE1203A = 199,
        CASE1204 = 200,
        CASE1204A = 201,
        CASE1205 = 202,
        CASE1205A = 203,
        CASE1206 = 204,
        CASE1206A = 205,
        CASE1207 = 206,
        CASE1207A = 207,
        CASE1208 = 208,
        CASE1208A = 209,
        CASE1209 = 210,
        CASE1209A = 211,
        CASE1210 = 212,
        CASE1210A = 213,
        CASE1211 = 214,
        CASE1211A = 215,
        CASE1212 = 216,
        CASE1212A = 217,
        CASE1213 = 218,
        CASE1213A = 219,
        CASE1226 = 220,
        CASE1227 = 221,
        CASE1214 = 222,
        CASE1215 = 223,
        CASE1216 = 224,
        CASE1217 = 225,
        CASE1220 = 226,
        CASE1220A = 227,
        CASE1221 = 228,
        CASE1221A = 229,
        CASE1222 = 230,
        CASE1222A = 231,
        CASE1223 = 232,
        CASE1439 = 233,
        CASE1439A = 234,
        CASE1440 = 235,
        CASE1440A = 236,
        CASE1441 = 237,
        CASE1441A = 238,
        CASE1442 = 239,
        CASE1443 = 240,
        CASE1444 = 241,
        CASE1445 = 242,
        CASE1446 = 243,
        CASE1445A = 244,
        CASE1446A = 245,
        CASE1067 = 246,
        CASE1067A = 247,
        CASE1068 = 248,
        CASE1068A = 249,
        CASE1136 = 250,
        CASE1136A = 251,
        CASE1449 = 252,
        CASE1450 = 253,
        CASE1451 = 254,
        CASE1452 = 255,
        CASE1459 = 256,
        CASE1459A = 257,
        CASE1460 = 258,
        CASE1460A = 259,
        CASE1137 = 260,
        CASE1455 = 261,
        CASE1455A = 262,
        CASE1456 = 263,
        CASE1456A = 264,
        CASE1117 = 265,
        CASE1117A = 266,
        CASE1457 = 267,
        CASE1358 = 268,
        CASE1358A = 269,
        CASE1359 = 270,
        CASE1359A = 271,
        CASE1360 = 272,
        CASE1360A = 273,
        CASE1361 = 274,
        CASE1361A = 275,
        CASE1362 = 276,
        CASE1362A = 277,
        CASE1363 = 278,
        CASE1363A = 279,
        CASE1364 = 280,
        CASE1365 = 281,
        CASE1366 = 282,
        CASE1366A = 283,
        CASE1367 = 284,
        CASE1367A = 285,
        CASE1058 = 286,
        CASE1063 = 287,
        CASE1063A = 288,
        CASE1368 = 289,
        CASE1368A = 290,
        CASE1369 = 291,
        CASE1369A = 292,
        CASE1372 = 293,
        CASE1373 = 294,
        CASE1374 = 295,
        CASE1375 = 296,
        CASE1376 = 297,
        CASE1150 = 298,
        CASE1150A = 299,
        CASE1151 = 300,
        CASE1151A = 301,
        CASE1152 = 302,
        CASE1152A = 303,
        CASE1153 = 304,
        CASE1153A = 305,
        CASE1154 = 306,
        CASE1154A = 307,
        CASE1155 = 308,
        CASE1155A = 309,
        CASE1156 = 310,
        CASE1156A = 311,
        CASE1172 = 312,
        CASE1172A = 313,
        CASE1174 = 314,
        CASE1174A = 315,
        CASE1170 = 316,
        CASE1170A = 317,
        CASE1171 = 318,
        CASE1171A = 319,
        CASE1176 = 320,
        CASE1176A = 321,
        CASE1177 = 322,
        CASE1177A = 323,
        CASE1163 = 324,
        CASE1164 = 325,
        CASE1165 = 326,
        CASE1166 = 327,
        CASE1161 = 328,
        CASE1161A = 329,
        CASE1162 = 330,
        CASE1162A = 331,
        CASE1323 = 332,
        CASE1324 = 333,
        CASE1322 = 334,
        CASE1040 = 335,
        CASE1040A = 336,
        CASE1041 = 337,
        CASE1041A = 338,
        CASE1022 = 339,
        CASE1022A = 340,
        CASE1023 = 341,
        CASE1023A = 342,
        CASE1042 = 343,
        CASE1042A = 344,
        CASE1024 = 345,
        CASE1024A = 346,
        CASE1025 = 347,
        CASE1025A = 348,
        CASE1056 = 349,
        CASE1057 = 350,
        CASE1074 = 351,
        CASE1078 = 352,
        CASE1092 = 353,
        CASE1092A = 354,
        CASE1093 = 355,
        CASE1093A = 356,
        CASE1094 = 357,
        CASE1094A = 358,
        CASE1095 = 359,
        CASE1095A = 360,
        CASE1096 = 361,
        CASE1096A = 362,
        CASE1097 = 363,
        CASE1097A = 364,
        CASE1098 = 365,
        CASE1098A = 366,
        CASE1099 = 367,
        CASE1099A = 368,
        CASE1100 = 369,
        CASE1100A = 370,
        CASE1101 = 371,
        CASE1101A = 372,
        CASE1224 = 373,
        CASE1225 = 374,
        CASE1500 = 375,
        CASE1501 = 377,
        CASE1502 = 378,
        CASE1500A = 379,
        CASE1501A = 380,
        CASE1502A = 381,
        CASE1503 = 382,
        CASE1503A = 383,
        CASE1504 = 384,
        CASE1504A = 385,
        CASE3000 = 386,
        CASE3001 = 387,
        CASE1124 = 388,
        CASE1139 = 389,
        CASE1149 = 390,
        CASE1419 = 391,
        CASE1420 = 392,
        CASE1421 = 393,
        CASE1422 = 394,
        CASE1423 = 395,
        CASE1424 = 396,
        CASE1426 = 397,
        CASE1427 = 398,
        CASE1428 = 399,
        CASE1429 = 400,
        CASE1430 = 401,
        CASE1431 = 402,
        CASE1435 = 403,
        CASE1436 = 404,
        CASE1437 = 405,
        CASE1425 = 406,
        CASE1433 = 409,
        CASE1433A = 410,
        CASE1436A = 411,
        CASE1437A = 412,
        CASE1800 = 413,
        CASE1801 = 414,
        CASE1802 = 415,
        CASE1803 = 416,
        CASE1800A = 417,
        CASE1801A = 418,
        CASE1802A = 419,
        CASE1803A = 420,
        CASE1435A = 421,
        CASE1430A = 422,
        CASE1431A = 423,
        CASE1426A = 424,
        CASE1850 = 425,
        CASE1851A = 426,
        CASE1851 = 427,
        CASE1852 = 428,
        CASE1195 = 429,
        CASE1195A = 430,
        CASE1197 = 431,
        CASE1197A = 432,
        CASE1199 = 433,
        CASE1199A = 434,
        CASE1178 = 435,
        CASE1178A = 436,
        CASE1181 = 437,
        CASE1181A = 438,
        CASE1182 = 439,
        CASE1182A = 440,
        CASE1183 = 441,
        CASE1183A = 442,
        CASE1180 = 443,
        CASE1180A = 444,
        CASE1179 = 445,
        CASE1179A = 446,
        CASE1184 = 447,
        CASE1184A = 448,
        CASE1185 = 449,
        CASE1185A = 450,
        CASE1186 = 451,
        CASE1186A = 452,
        CASE1187 = 453,
        CASE1187A = 454,
        CASE1190 = 455,
        CASE1191 = 456,
        CASE1192 = 457,
        CASE1193 = 458,
        CASE1194 = 459,
        CASE1194A = 460,
        CASE1196 = 461,
        CASE1198 = 462,
        CASE1196A = 463,
        CASE1198A = 464,
        CASE1189 = 465,
        CASE1200 = 466,
        CASE1038 = 467,
        CASE1038A = 468,
        CASE1325 = 469,
        CASE1326 = 470,
        CASE1327 = 471,
        CASE1122 = 472,
        CASE1123 = 473,
        CASE1122A = 474,
        CASE1123A = 475,
        CASE1355 = 476,
        CASE1357 = 477,
        CASE1107 = 448,
        CASE1110 = 449,
        CASE1752 = 451,
        CASE1752A = 452,
        CASE1753 = 453,
        CASE1753A = 454,
        CASE1754 = 455,
        CASE1754A = 456,
        CASE1717 = 457,
        CASE1718 = 458,
        CASE1719 = 459,
        CASE1717A = 460,
        CASE1718A = 461,
        CASE1719A = 462,
        CASE1767 = 463,
        CASE1767A = 464,
        CASE1727 = 465,
        CASE1727A = 466,
        CASE1728 = 467,
        CASE1728A = 468,
        CASE1723 = 469,
        CASE1723A = 470,
        CASE1724 = 471,
        CASE1724A = 472,
        CASE1725 = 473,
        CASE1725A = 474,
        CASE1726 = 475,
        CASE1726A = 476,
        CASE1729 = 477,
        CASE1729A = 478,
        CASE1731 = 479,
        CASE1731A = 480,
        CASE1770 = 481,
        CASE1770A = 482,
        CASE1771 = 483,
        CASE1771A = 484,
        CASE1772 = 485,
        CASE1772A = 486,
        CASE1750 = 487,
        CASE1750A = 488,
        CASE1751 = 489,
        CASE1751A = 490,
        CASE1432 = 491,
        CASE1432A = 492,
        CASE2647 = 493,
        CASE2647A = 494,
        CASE2648 = 495,
        CASE2648A = 496,
        CASE2650 = 497,
        CASE2650A = 498,
        CASE2652 = 499,
        CASE2652A = 500,
        CASE2654 = 501,
        CASE2654A = 502,
        CASE2657 = 503,
        CASE2657A = 504,
        CASE2661 = 505,
        CASE2661A = 506,
        CASE2665 = 507,
        CASE2665A = 508,
        CASE2666 = 509,
        CASE2666A = 510,
        CASE2668 = 511,
        CASE2668A = 512,
        CASE2669 = 513,
        CASE2669A = 514,
        CASE2670 = 515,
        CASE2670A = 516,
        CASE2673 = 517,
        CASE2673A = 518,
        CASE2674 = 519,
        CASE2674A = 520,
        CASE2675 = 512,
        CASE2675A = 513,
        CASE2676 = 514,
        CASE2676A = 515,
        CASE2677 = 516,
        CASE2677A = 517,
        CASE2678 = 518,
        CASE2678A = 519,
        CASE2706 = 520,
        CASE2706A = 521,
        CASE1642 = 522,
        CASE1642A = 523,
        CASE1643 = 524,
        CASE1643A = 525,
        CASE1644 = 526,
        CASE1644A = 527,
        CASE1715 = 528,
        CASE1716 = 529,
        CASE1714 = 530,
        CASE1714A = 531,
        CASE1715A = 532,
        CASE1716A = 533,
        CASE1768 = 534,
        CASE1768A = 535,
        CASE1769 = 536,
        CASE1769A = 537,
        CASE1745 = 538,
        CASE1026 = 539,
        CASE1026A = 540,
        CASE1027 = 541,
        CASE1027A = 542,
        CASE1028 = 543,
        CASE1028A = 544,
        CASE1046 = 545,
        CASE1138 = 545,
        CASE1138A = 546,
        CASE1054 = 547,
        CASE1055 = 548,
    }
}