/* SportPitShop - SQL Server full database with data (11 tables) */
IF DB_ID(N'SportPitShop') IS NOT NULL BEGIN
  ALTER DATABASE SportPitShop SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
  DROP DATABASE SportPitShop;
END
GO
CREATE DATABASE SportPitShop;
GO
USE SportPitShop;
GO

CREATE TABLE Role (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(100) NOT NULL UNIQUE);
CREATE TABLE [User] (Id INT IDENTITY PRIMARY KEY, Surname NVARCHAR(100), Name NVARCHAR(100), Patronymic NVARCHAR(100), Login NVARCHAR(150) NOT NULL UNIQUE, Password NVARCHAR(150) NOT NULL, RoleId INT NOT NULL FOREIGN KEY REFERENCES Role(Id));
CREATE TABLE Producer (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(150) NOT NULL UNIQUE);
CREATE TABLE Category (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(150) NOT NULL UNIQUE);
CREATE TABLE Unit (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(50) NOT NULL UNIQUE);
CREATE TABLE Provider (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(150) NOT NULL UNIQUE);
CREATE TABLE Product (
    Id INT IDENTITY PRIMARY KEY,
    Article NVARCHAR(100) NOT NULL UNIQUE,
    Name NVARCHAR(200) NOT NULL,
    UnitID INT NOT NULL FOREIGN KEY REFERENCES Unit(Id),
    Price INT NOT NULL,
    ProviderID INT NOT NULL FOREIGN KEY REFERENCES Provider(Id),
    ProducerID INT NOT NULL FOREIGN KEY REFERENCES Producer(Id),
    CategoryID INT NOT NULL FOREIGN KEY REFERENCES Category(Id),
    Discount INT NOT NULL DEFAULT 0,
    AmountInStock INT NOT NULL DEFAULT 0,
    Description NVARCHAR(MAX) NULL,
    Photo NVARCHAR(200) NULL
);
CREATE TABLE PickUpPoint (
    Id INT IDENTITY PRIMARY KEY,
    PostCode NVARCHAR(20) NULL,
    City NVARCHAR(100) NULL,
    Street NVARCHAR(150) NULL,
    Building NVARCHAR(50) NULL,
    FullAddress NVARCHAR(300) NULL
);
CREATE TABLE OrderStatus (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(100) NOT NULL UNIQUE);
CREATE TABLE [Order] (
    Id INT IDENTITY PRIMARY KEY,
    CreationDate DATE NOT NULL,
    DeliveryDate DATE NULL,
    PickUpPointId INT NOT NULL FOREIGN KEY REFERENCES PickUpPoint(Id),
    UserID INT NOT NULL FOREIGN KEY REFERENCES [User](Id),
    ReceiptCode NVARCHAR(20) NULL,
    StatusId INT NOT NULL FOREIGN KEY REFERENCES OrderStatus(Id)
);
CREATE TABLE ProductInOrder (
    Id INT IDENTITY PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    Amount INT NOT NULL
);
GO

INSERT INTO Role(Name) VALUES (N'Авторизированный клиент');
INSERT INTO Role(Name) VALUES (N'Администратор');
INSERT INTO Role(Name) VALUES (N'Клиент');
INSERT INTO Role(Name) VALUES (N'Менеджер');
INSERT INTO Producer(Name) VALUES (N'BSN');
INSERT INTO Producer(Name) VALUES (N'Carnivor');
INSERT INTO Producer(Name) VALUES (N'Cellucor');
INSERT INTO Producer(Name) VALUES (N'Dymatize');
INSERT INTO Producer(Name) VALUES (N'FitMiss');
INSERT INTO Producer(Name) VALUES (N'GU Energy');
INSERT INTO Producer(Name) VALUES (N'Grenade');
INSERT INTO Producer(Name) VALUES (N'Mars');
INSERT INTO Producer(Name) VALUES (N'Maxler');
INSERT INTO Producer(Name) VALUES (N'Multipower');
INSERT INTO Producer(Name) VALUES (N'Muscle Milk');
INSERT INTO Producer(Name) VALUES (N'MuscleTech');
INSERT INTO Producer(Name) VALUES (N'Myprotein');
INSERT INTO Producer(Name) VALUES (N'Now Foods');
INSERT INTO Producer(Name) VALUES (N'Nuun');
INSERT INTO Producer(Name) VALUES (N'Optimum Nutrition');
INSERT INTO Producer(Name) VALUES (N'OstroVit');
INSERT INTO Producer(Name) VALUES (N'Protein+');
INSERT INTO Producer(Name) VALUES (N'SAN');
INSERT INTO Producer(Name) VALUES (N'Scitec Nutrition');
INSERT INTO Producer(Name) VALUES (N'Sports Research');
INSERT INTO Producer(Name) VALUES (N'Transparent Labs');
INSERT INTO Producer(Name) VALUES (N'Universal Nutrition');
INSERT INTO Producer(Name) VALUES (N'Vega');
INSERT INTO Category(Name) VALUES (N'Аминокислоты');
INSERT INTO Category(Name) VALUES (N'Витамины');
INSERT INTO Category(Name) VALUES (N'Витамины и минералы');
INSERT INTO Category(Name) VALUES (N'Гейнеры');
INSERT INTO Category(Name) VALUES (N'Гидратация');
INSERT INTO Category(Name) VALUES (N'Готовые коктейли');
INSERT INTO Category(Name) VALUES (N'Жиросжигатели');
INSERT INTO Category(Name) VALUES (N'Изолят протеина');
INSERT INTO Category(Name) VALUES (N'Казеин');
INSERT INTO Category(Name) VALUES (N'Креатин');
INSERT INTO Category(Name) VALUES (N'Минералы');
INSERT INTO Category(Name) VALUES (N'Предтренировочные комплексы');
INSERT INTO Category(Name) VALUES (N'Протеиновые батончики');
INSERT INTO Category(Name) VALUES (N'Протеиновые десерты');
INSERT INTO Category(Name) VALUES (N'Протеиновые сладости');
INSERT INTO Category(Name) VALUES (N'Протеины для женщин');
INSERT INTO Category(Name) VALUES (N'Растительные протеины');
INSERT INTO Category(Name) VALUES (N'Специальные протеины');
INSERT INTO Category(Name) VALUES (N'Сывороточный протеин');
INSERT INTO Category(Name) VALUES (N'Энергетика');
INSERT INTO Unit(Name) VALUES (N'банка');
INSERT INTO Unit(Name) VALUES (N'мешок');
INSERT INTO Unit(Name) VALUES (N'уп.');
INSERT INTO Provider(Name) VALUES (N'СпортНутри');
INSERT INTO Provider(Name) VALUES (N'ФитПродукт');
INSERT INTO Provider(Name) VALUES (N'Эко Спорт');
INSERT INTO OrderStatus(Name) VALUES (N'Завершен');
INSERT INTO OrderStatus(Name) VALUES (N'Новый');
GO

SET IDENTITY_INSERT PickUpPoint ON;
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (1, N'620014', N'Екатеринбург', N'ул. Ленина', N'52', N'620014, г. Екатеринбург, ул. Ленина, 52, ТЦ "СпортМаркет", 1 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (2, N'620076', N'Екатеринбург', N'пр. Космонавтов', N'18', N'620076, г. Екатеринбург, пр. Космонавтов, 18, м. "Динамо", вход с торца');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (3, N'620026', N'Екатеринбург', N'ул. 8 Марта', N'8', N'620026, г. Екатеринбург, ул. 8 Марта, 8, ТЦ "Гринвич", -1 этаж, пав. 45');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (4, N'620144', N'Екатеринбург', N'ул. Мира', N'34', N'620144, г. Екатеринбург, ул. Мира, 34, Спортивный комплекс "Олимп", ресепшн');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (5, N'620017', N'Екатеринбург', N'ул. Чапаева', N'65', N'620017, г. Екатеринбург, ул. Чапаева, 65, ТЦ "Пассаж", 2 этаж, рядом с лифтом');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (6, N'620041', N'Екатеринбург', N'ул. Шевченко', N'31', N'620041, г. Екатеринбург, ул. Шевченко, 31, фитнес-клуб "Железный характер"');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (7, N'620072', N'Екатеринбург', N'ул. Сурикова', N'29', N'620072, г. Екатеринбург, ул. Сурикова, 29, ТЦ "Радуга", пав. 12');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (8, N'620020', N'Екатеринбург', N'ул. Малышева', N'74', N'620020, г. Екатеринбург, ул. Малышева, 74, ТЦ "Алатырь", 1 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (9, N'620012', N'Екатеринбург', N'ул. Гоголя', N'27', N'620012, г. Екатеринбург, ул. Гоголя, 27, м. "Геологическая", выход №3');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (10, N'620100', N'Екатеринбург', N'ул. Сакко и Ванцетти', N'43', N'620100, г. Екатеринбург, ул. Сакко и Ванцетти, 43, ТЦ "Кольцо", -1 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (11, N'620085', N'Екатеринбург', N'ул. Татищева', N'41', N'620085, г. Екатеринбург, ул. Татищева, 41, фитнес-студия "Формула тела"');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (12, N'620043', N'Екатеринбург', N'ул. Хохрякова', N'82', N'620043, г. Екатеринбург, ул. Хохрякова, 82, ТЦ "Верх-Исетский", пав. 78');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (13, N'620075', N'Екатеринбург', N'ул. Фрунзе', N'37', N'620075, г. Екатеринбург, ул. Фрунзе, 37, м. "Площадь 1905 года", выход №2');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (14, N'620016', N'Екатеринбург', N'ул. Свердлова', N'23', N'620016, г. Екатеринбург, ул. Свердлова, 23, ТЦ "ЦУМ", 3 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (15, N'620023', N'Екатеринбург', N'ул. Декабристов', N'52', N'620023, г. Екатеринбург, ул. Декабристов, 52, ТЦ "Мегаполис", пав. 103');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (16, N'620078', N'Екатеринбург', N'ул. Карла Либкнехта', N'55', N'620078, г. Екатеринбург, ул. Карла Либкнехта, 55, спортивный магазин "Спортмастер"');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (17, N'620049', N'Екатеринбург', N'ул. Куйбышева', N'94', N'620049, г. Екатеринбург, ул. Куйбышева, 94, ТЦ "Аквамарин", 1 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (18, N'620007', N'Екатеринбург', N'ул. 8 Марта', N'15', N'620007, г. Екатеринбург, ул. 8 Марта, 15, фитнес-клуб "Титан"');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (19, N'620021', N'Екатеринбург', N'ул. Амундсена', N'63', N'620021, г. Екатеринбург, ул. Амундсена, 63, ТЦ "Радуга-Парк", пав. 29');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (20, N'620019', N'Екатеринбург', N'ул. Бориса Ельцина', N'45', N'620019, г. Екатеринбург, ул. Бориса Ельцина, 45, ТЦ "Ельцин Центр", -2 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (21, N'620083', N'Екатеринбург', N'ул. Шарташская', N'30', N'620083, г. Екатеринбург, ул. Шарташская, 30, м. "Ботаническая", выход №1');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (22, N'620073', N'Екатеринбург', N'ул. Репина', N'57', N'620073, г. Екатеринбург, ул. Репина, 57, ТЦ "Мега-Дизайн", пав. 64');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (23, N'620045', N'Екатеринбург', N'ул. Студенческая', N'39', N'620045, г. Екатеринбург, ул. Студенческая, 39, студенческий спортивный клуб УрФУ');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (24, N'620018', N'Екатеринбург', N'ул. Вайнера', N'22', N'620018, г. Екатеринбург, ул. Вайнера, 22, ТЦ "Пионер", 2 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (25, N'620077', N'Екатеринбург', N'ул. Комсомольская', N'71', N'620077, г. Екатеринбург, ул. Комсомольская, 71, м. "Чкаловская", выход №4');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (26, N'620042', N'Екатеринбург', N'ул. Фурманова', N'35', N'620042, г. Екатеринбург, ул. Фурманова, 35, ТЦ "Комсомолл", пав. 91');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (27, N'620081', N'Екатеринбург', N'ул. Шевченко', N'99', N'620081, г. Екатеринбург, ул. Шевченко, 99, фитнес-клуб "Золотой стандарт"');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (28, N'620024', N'Екатеринбург', N'ул. Техническая', N'26', N'620024, г. Екатеринбург, ул. Техническая, 26, ТЦ "Техно", 1 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (29, N'620079', N'Екатеринбург', N'ул. Победы', N'61', N'620079, г. Екатеринбург, ул. Победы, 61, ТЦ "Верх-Исетский", пав. 112');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (30, N'620015', N'Екатеринбург', N'ул. Луначарского', N'47', N'620015, г. Екатеринбург, ул. Луначарского, 47, м. "Уралмаш", выход №2');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (31, N'620084', N'Екатеринбург', N'ул. Бебеля', N'80', N'620084, г. Екатеринбург, ул. Бебеля, 80, ТЦ "Аура", 2 этаж');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (32, N'620047', N'Екатеринбург', N'ул. Сакко и Ванцетти', N'92', N'620047, г. Екатеринбург, ул. Сакко и Ванцетти, 92, ТЦ "Гринвич-2", пав. 37');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (33, N'620022', N'Екатеринбург', N'ул. Чкалова', N'24', N'620022, г. Екатеринбург, ул. Чкалова, 24, фитнес-студия "Прокачка"');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (34, N'620074', N'Екатеринбург', N'ул. Крауля', N'53', N'620074, г. Екатеринбург, ул. Крауля, 53, ТЦ "Радуга", пав. 88');
INSERT INTO PickUpPoint(Id,PostCode,City,Street,Building,FullAddress) VALUES (35, N'620013', N'Екатеринбург', N'ул. Малышева', N'101', N'620013, г. Екатеринбург, ул. Малышева, 101, м. "Площадь 1905 года", выход №5');
SET IDENTITY_INSERT PickUpPoint OFF;
GO

SET IDENTITY_INSERT [User] ON;
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (1, N'Козлов', N'Иван', N'Сергеевич', N'admin@sportpit.ru', N'Adm1n!2026', (SELECT Id FROM Role WHERE Name=N'Администратор'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (2, N'Петрова', N'Анастасия', N'Дмитриевна', N'apetrova@sportpit.ru', N'P3tr0v@_Sp', (SELECT Id FROM Role WHERE Name=N'Администратор'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (3, N'Соколов', N'Артем', N'Владимирович', N'asokolov@sportpit.ru', N'S0k0l0v#Sp', (SELECT Id FROM Role WHERE Name=N'Администратор'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (4, N'Васильев', N'Дмитрий', N'Андреевич', N'dvasiliev@sportpit.ru', N'V@s1l13v!', (SELECT Id FROM Role WHERE Name=N'Менеджер'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (5, N'Морозова', N'Екатерина', N'Олеговна', N'emorozova@sportpit.ru', N'M0r0z0v@26', (SELECT Id FROM Role WHERE Name=N'Менеджер'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (6, N'Новиков', N'Алексей', N'Павлович', N'anovikov@sportpit.ru', N'N0v1k0v_Sp', (SELECT Id FROM Role WHERE Name=N'Менеджер'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (7, N'Смирнов', N'Максим', N'Игоревич', N'm.smirnov@gmail.com', N'Sm1rn0v!2026', (SELECT Id FROM Role WHERE Name=N'Авторизированный клиент'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (8, N'Кузнецова', N'Анна', N'Викторовна', N'a.kuznetsova@mail.ru', N'Kuzn3c0v@', (SELECT Id FROM Role WHERE Name=N'Авторизированный клиент'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (9, N'Иванов', N'Роман', N'Сергеевич', N'r.ivanov@yandex.ru', N'1v@n0v_Sp', (SELECT Id FROM Role WHERE Name=N'Авторизированный клиент'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (10, N'Лебедева', N'Ольга', N'Дмитриевна', N'o.lebedeva@mail.ru', N'L3b3d3v@26', (SELECT Id FROM Role WHERE Name=N'Авторизированный клиент'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (11, N'Иванов', N'Роман', N'Сергеевич', N'client1@sportpit.local', N'Client!2026', (SELECT Id FROM Role WHERE Name=N'Клиент'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (12, N'Кузнецова', N'Анна', N'Викторовна', N'client2@sportpit.local', N'Client!2026', (SELECT Id FROM Role WHERE Name=N'Клиент'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (13, N'Лебедева', N'Ольга', N'Дмитриевна', N'client3@sportpit.local', N'Client!2026', (SELECT Id FROM Role WHERE Name=N'Клиент'));
INSERT INTO [User](Id,Surname,Name,Patronymic,Login,Password,RoleId) VALUES (14, N'Смирнов', N'Максим', N'Игоревич', N'client4@sportpit.local', N'Client!2026', (SELECT Id FROM Role WHERE Name=N'Клиент'));
SET IDENTITY_INSERT [User] OFF;
GO

SET IDENTITY_INSERT Product ON;
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
1,
N'SP100P1',
N'Протеин Gold Standard 100% Whey 2.27кг',
(SELECT Id FROM Unit WHERE Name=N'банка'),
3490,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Optimum Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Сывороточный протеин'),
5,
45,
N'Классический сывороточный протеин, ванильный вкус',
N'1.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
2,
N'SP200G1',
N'Гейнер Mass Tech 4.54кг',
(SELECT Id FROM Unit WHERE Name=N'банка'),
4290,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'MuscleTech'),
(SELECT Id FROM Category WHERE Name=N'Гейнеры'),
8,
32,
N'Высокоуглеводный гейнер для набора массы, шоколад',
N'2.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
3,
N'SP300B1',
N'BCAA Amino X 30 порций',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1890,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Optimum Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Аминокислоты'),
3,
68,
N'Комплекс аминокислот с электролитами, фруктовый пунш',
N'3.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
4,
N'SP400C1',
N'Креатин моногидрат 300г',
(SELECT Id FROM Unit WHERE Name=N'банка'),
1290,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'Scitec Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Креатин'),
0,
54,
N'Чистый креатин моногидрат, 300 порций',
N'4.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
5,
N'SP500P2',
N'Изолят протеина 1кг',
(SELECT Id FROM Unit WHERE Name=N'банка'),
2790,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Myprotein'),
(SELECT Id FROM Category WHERE Name=N'Изолят протеина'),
7,
29,
N'Протеин с минимальным содержанием жира и лактозы, клубника',
N'5.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
6,
N'SP600V1',
N'Витаминный комплекс Animal Pak 22 порции',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
2490,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'Universal Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Витамины и минералы'),
4,
41,
N'Профессиональный витаминный комплекс для атлетов',
N'6.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
7,
N'SP700P3',
N'Казеиновый протеин 907г',
(SELECT Id FROM Unit WHERE Name=N'банка'),
2590,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Optimum Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Казеин'),
6,
37,
N'Медленный протеин для ночного приема, шоколадный крем',
N'7.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
8,
N'SP800F1',
N'L-карнитин 1000мг 120 капсул',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1450,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'SAN'),
(SELECT Id FROM Category WHERE Name=N'Жиросжигатели'),
10,
85,
N'Транспортировщик жирных кислот, поддержка метаболизма',
N'8.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
9,
N'SP900P4',
N'Растительный протеин 1.5кг',
(SELECT Id FROM Unit WHERE Name=N'банка'),
2890,
(SELECT Id FROM Provider WHERE Name=N'Эко Спорт'),
(SELECT Id FROM Producer WHERE Name=N'Vega'),
(SELECT Id FROM Category WHERE Name=N'Растительные протеины'),
12,
23,
N'Протеин на основе гороха и риса, ваниль',
N'9.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
10,
N'SP1000E1',
N'Предтреник C4 Original 30 порций',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
2190,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Cellucor'),
(SELECT Id FROM Category WHERE Name=N'Предтренировочные комплексы'),
5,
51,
N'Энергия, фокус, пампинг перед тренировкой, арбуз',
N'10.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
11,
N'SP1100G2',
N'Гейнер True-Mass 2.72кг',
(SELECT Id FROM Unit WHERE Name=N'банка'),
3890,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'BSN'),
(SELECT Id FROM Category WHERE Name=N'Гейнеры'),
9,
18,
N'Сбалансированный гейнер 50/50 белки-углеводы, ванильный кекс',
N'11.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
12,
N'SP1200Z1',
N'ZMA 90 капсул',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1690,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Optimum Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Минералы'),
4,
63,
N'Цинк, магний, витамин В6 для восстановления',
N'12.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
13,
N'SP1300P5',
N'Коллагеновый протеин 500г',
(SELECT Id FROM Unit WHERE Name=N'банка'),
2390,
(SELECT Id FROM Provider WHERE Name=N'Эко Спорт'),
(SELECT Id FROM Producer WHERE Name=N'Sports Research'),
(SELECT Id FROM Category WHERE Name=N'Специальные протеины'),
7,
0,
N'Поддержка суставов и связок, без вкуса',
N'13.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
14,
N'SP1400B2',
N'BCAA 4:1:1 400г',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1590,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'Scitec Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Аминокислоты'),
6,
72,
N'Соотношение лейцин:изолейцин:валин 4:1:1, лимон',
N'14.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
15,
N'SP1500P6',
N'Протеиновый батончик 60г × 12 шт',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
890,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Mars'),
(SELECT Id FROM Category WHERE Name=N'Протеиновые батончики'),
15,
120,
N'Удобный перекус с 20г белка, шоколадно-арахисовый',
N'15.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
16,
N'SP1600C2',
N'Креатин этил эфир 120 капсул',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1790,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'BSN'),
(SELECT Id FROM Category WHERE Name=N'Креатин'),
3,
44,
N'Улучшенная форма креатина с высокой биодоступностью',
N'16.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
17,
N'SP1700E2',
N'Энергетический гель 60мл × 20 шт',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1290,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'GU Energy'),
(SELECT Id FROM Category WHERE Name=N'Энергетика'),
8,
0,
N'Для поддержки во время длительных тренировок',
N'17.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
18,
N'SP1800P7',
N'Говяжий протеин 907г',
(SELECT Id FROM Unit WHERE Name=N'банка'),
3190,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'Carnivor'),
(SELECT Id FROM Category WHERE Name=N'Специальные протеины'),
5,
26,
N'Протеин из говядины, без лактозы, шоколад',
N'18.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
19,
N'SP1900F2',
N'CLA 1000мг 120 капсул',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1990,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Optimum Nutrition'),
(SELECT Id FROM Category WHERE Name=N'Жиросжигатели'),
7,
58,
N'Конъюгированная линолевая кислота для контроля веса',
N'19.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
20,
N'SP2000P8',
N'Протеиновый коктейль RTD 330мл × 12 шт',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1490,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'Muscle Milk'),
(SELECT Id FROM Category WHERE Name=N'Готовые коктейли'),
10,
95,
N'Готовый протеиновый напиток, шоколад',
N'20.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
21,
N'SP2100G3',
N'Гейнер весовой 5кг',
(SELECT Id FROM Unit WHERE Name=N'мешок'),
5490,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Maxler'),
(SELECT Id FROM Category WHERE Name=N'Гейнеры'),
12,
15,
N'Максимальная калорийность для быстрого набора массы',
N'21.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
22,
N'SP2200V2',
N'Витамин D3+K2 60 капсул',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
990,
(SELECT Id FROM Provider WHERE Name=N'Эко Спорт'),
(SELECT Id FROM Producer WHERE Name=N'Now Foods'),
(SELECT Id FROM Category WHERE Name=N'Витамины'),
5,
130,
N'Поддержка костной системы и иммунитета',
N'22.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
23,
N'SP2300P9',
N'Протеин для женщин 1.36кг',
(SELECT Id FROM Unit WHERE Name=N'банка'),
2690,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'FitMiss'),
(SELECT Id FROM Category WHERE Name=N'Протеины для женщин'),
8,
43,
N'С добавками для красоты кожи и волос, ягоды',
N'23.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
24,
N'SP2400E3',
N'Предтреник без кофеина 25 порций',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
2390,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Transparent Labs'),
(SELECT Id FROM Category WHERE Name=N'Предтренировочные комплексы'),
6,
65,
N'Для вечерних тренировок, фруктовый вкус',
N'24.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
25,
N'SP2500B3',
N'Электролиты 30 порций',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1190,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'Nuun'),
(SELECT Id FROM Category WHERE Name=N'Гидратация'),
4,
78,
N'Таблетки для воды с электролитами, лимон-лайм',
N'25.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
26,
N'SP2600P10',
N'Протеиновый пудинг 450г',
(SELECT Id FROM Unit WHERE Name=N'банка'),
1590,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Multipower'),
(SELECT Id FROM Category WHERE Name=N'Протеиновые десерты'),
9,
28,
N'Готовый протеиновый десерт, шоколадный',
N'26.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
27,
N'SP2700C3',
N'Креатин+ТАУРИН 200г',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1390,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'OstroVit'),
(SELECT Id FROM Category WHERE Name=N'Креатин'),
7,
64,
N'Комбинированный комплекс для силы и выносливости',
N'27.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
28,
N'SP2800F3',
N'Термогеник 60 капсул',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
2290,
(SELECT Id FROM Provider WHERE Name=N'СпортНутри'),
(SELECT Id FROM Producer WHERE Name=N'Grenade'),
(SELECT Id FROM Category WHERE Name=N'Жиросжигатели'),
11,
51,
N'Комплекс для ускорения метаболизма',
N'28.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
29,
N'SP2900S1',
N'Сывороточный изолят 2.27кг',
(SELECT Id FROM Unit WHERE Name=N'банка'),
4590,
(SELECT Id FROM Provider WHERE Name=N'ФитПродукт'),
(SELECT Id FROM Producer WHERE Name=N'Dymatize'),
(SELECT Id FROM Category WHERE Name=N'Изолят протеина'),
8,
35,
N'Ультрачистый изолят, 26г белка на порцию',
N'29.jpg'
);
INSERT INTO Product(
Id,Article,Name,UnitID,Price,ProviderID,ProducerID,CategoryID,Discount,AmountInStock,Description,Photo
) VALUES (
30,
N'SP3000P11',
N'Протеиновый шоколад 85г × 10 шт',
(SELECT Id FROM Unit WHERE Name=N'уп.'),
1090,
(SELECT Id FROM Provider WHERE Name=N'Эко Спорт'),
(SELECT Id FROM Producer WHERE Name=N'Protein+'),
(SELECT Id FROM Category WHERE Name=N'Протеиновые сладости'),
6,
71,
N'Шоколад с 15г белка, без сахара',
N'30.jpg'
);
SET IDENTITY_INSERT Product OFF;
GO

SET IDENTITY_INSERT [Order] ON;
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
1,
'2025-01-15',
'2025-01-20',
3,
14,
N'701',
(SELECT Id FROM OrderStatus WHERE Name=N'Завершен')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
2,
'2025-01-18',
'2025-01-22',
7,
12,
N'702',
(SELECT Id FROM OrderStatus WHERE Name=N'Завершен')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
3,
'2025-01-20',
'2025-01-24',
12,
11,
N'703',
(SELECT Id FROM OrderStatus WHERE Name=N'Завершен')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
4,
'2025-01-22',
'2025-01-26',
5,
13,
N'704',
(SELECT Id FROM OrderStatus WHERE Name=N'Завершен')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
5,
'2025-01-25',
'2025-01-28',
3,
14,
N'705',
(SELECT Id FROM OrderStatus WHERE Name=N'Завершен')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
6,
'2025-01-28',
'2025-01-30',
18,
12,
N'706',
(SELECT Id FROM OrderStatus WHERE Name=N'Завершен')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
7,
'2025-01-30',
'2025-02-02',
9,
11,
N'707',
(SELECT Id FROM OrderStatus WHERE Name=N'Завершен')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
8,
'2025-02-02',
'2025-02-05',
22,
13,
N'708',
(SELECT Id FROM OrderStatus WHERE Name=N'Новый')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
9,
'2025-02-04',
'2025-02-07',
14,
14,
N'709',
(SELECT Id FROM OrderStatus WHERE Name=N'Новый')
);
INSERT INTO [Order](
Id,CreationDate,DeliveryDate,PickUpPointId,UserID,ReceiptCode,StatusId
) VALUES (
10,
'2025-02-05',
'2025-02-09',
22,
12,
N'710',
(SELECT Id FROM OrderStatus WHERE Name=N'Новый')
);
SET IDENTITY_INSERT [Order] OFF;
GO

INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (1, (SELECT Id FROM Product WHERE Article=N'SP100P1'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (1, (SELECT Id FROM Product WHERE Article=N'SP300B1'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (2, (SELECT Id FROM Product WHERE Article=N'SP400C1'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (2, (SELECT Id FROM Product WHERE Article=N'SP600V1'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (3, (SELECT Id FROM Product WHERE Article=N'SP500P2'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (3, (SELECT Id FROM Product WHERE Article=N'SP800F1'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (4, (SELECT Id FROM Product WHERE Article=N'SP700P3'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (4, (SELECT Id FROM Product WHERE Article=N'SP1400B2'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (5, (SELECT Id FROM Product WHERE Article=N'SP100P1'), 3);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (5, (SELECT Id FROM Product WHERE Article=N'SP1500P6'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (6, (SELECT Id FROM Product WHERE Article=N'SP200G1'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (6, (SELECT Id FROM Product WHERE Article=N'SP1100G2'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (7, (SELECT Id FROM Product WHERE Article=N'SP900P4'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (7, (SELECT Id FROM Product WHERE Article=N'SP1200Z1'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (8, (SELECT Id FROM Product WHERE Article=N'SP1000E1'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (8, (SELECT Id FROM Product WHERE Article=N'SP2400E3'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (9, (SELECT Id FROM Product WHERE Article=N'SP1900F2'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (9, (SELECT Id FROM Product WHERE Article=N'SP2200V2'), 2);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (10, (SELECT Id FROM Product WHERE Article=N'SP2900S1'), 1);
INSERT INTO ProductInOrder(OrderId,ProductId,Amount) VALUES (10, (SELECT Id FROM Product WHERE Article=N'SP3000P11'), 3);
GO

-- Checks
SELECT 'Tables' AS Info, COUNT(*) AS Cnt FROM sys.tables;
SELECT 'Products' AS Info, COUNT(*) AS Cnt FROM Product;
SELECT 'Users' AS Info, COUNT(*) AS Cnt FROM [User];
SELECT 'Orders' AS Info, COUNT(*) AS Cnt FROM [Order];