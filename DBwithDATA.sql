USE [MasterFloor]
GO
/****** Object:  Table [dbo].[Material_type]    Script Date: 19.12.24 20:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Material_type](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[ScrapRate] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Partner_products]    Script Date: 19.12.24 20:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Partner_products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Product] [int] NOT NULL,
	[Partner] [int] NOT NULL,
	[Quantity] [int] NULL,
	[SaleDATE] [date] NULL,
	[Discount] [decimal](3, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Partner_type]    Script Date: 19.12.24 20:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Partner_type](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Partners]    Script Date: 19.12.24 20:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Partners](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Director] [nvarchar](120) NOT NULL,
	[Email] [nvarchar](80) NULL,
	[Number] [char](12) NULL,
	[Address] [nvarchar](150) NOT NULL,
	[INN] [char](10) NULL,
	[Rating] [int] NULL,
 CONSTRAINT [PK__Partners__3214EC27BA9CF023] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_type]    Script Date: 19.12.24 20:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_type](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[RatioTypeProduct] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 19.12.24 20:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Number] [char](10) NOT NULL,
	[MinCost] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Material_type] ON 

INSERT [dbo].[Material_type] ([ID], [Type], [ScrapRate]) VALUES (1, N'Тип материала 1', 0.001)
INSERT [dbo].[Material_type] ([ID], [Type], [ScrapRate]) VALUES (2, N'Тип материала 2', 0.0095)
INSERT [dbo].[Material_type] ([ID], [Type], [ScrapRate]) VALUES (3, N'Тип материала 3', 0.0028)
INSERT [dbo].[Material_type] ([ID], [Type], [ScrapRate]) VALUES (4, N'Тип материала 4', 0.0055)
INSERT [dbo].[Material_type] ([ID], [Type], [ScrapRate]) VALUES (5, N'Тип материала 5', 0.0034)
SET IDENTITY_INSERT [dbo].[Material_type] OFF
GO
SET IDENTITY_INSERT [dbo].[Partner_products] ON 

INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (1, 1, 1, 15500, CAST(N'2023-03-23' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (2, 3, 1, 12350, CAST(N'2023-12-18' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (3, 4, 1, 37400, CAST(N'2024-06-07' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (4, 2, 2, 35000, CAST(N'2022-12-02' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (5, 5, 2, 1250, CAST(N'2023-05-17' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (6, 3, 2, 1000, CAST(N'2024-06-07' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (7, 1, 2, 7550, CAST(N'2024-07-01' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (8, 1, 3, 7250, CAST(N'2023-01-22' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (9, 2, 3, 2500, CAST(N'2024-07-05' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (10, 4, 4, 59050, CAST(N'2023-03-20' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (11, 3, 4, 37200, CAST(N'2024-03-12' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (12, 5, 4, 4500, CAST(N'2024-05-14' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (13, 3, 11, 50000, CAST(N'2023-09-19' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (14, 4, 11, 670000, CAST(N'2023-11-10' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (15, 1, 11, 35000, CAST(N'2024-04-15' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (16, 2, 11, 25000, CAST(N'2024-06-12' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (17, 1, 12, 50000, CAST(N'2024-12-17' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (18, 2, 13, 100000, CAST(N'2024-12-17' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (19, 2, 14, 50000, CAST(N'2024-12-17' AS Date), NULL)
INSERT [dbo].[Partner_products] ([ID], [Product], [Partner], [Quantity], [SaleDATE], [Discount]) VALUES (20, 3, 15, 200000, CAST(N'2024-12-17' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[Partner_products] OFF
GO
SET IDENTITY_INSERT [dbo].[Partner_type] ON 

INSERT [dbo].[Partner_type] ([ID], [Type]) VALUES (1, N'ЗАО')
INSERT [dbo].[Partner_type] ([ID], [Type]) VALUES (2, N'ООО')
INSERT [dbo].[Partner_type] ([ID], [Type]) VALUES (3, N'ПАО')
INSERT [dbo].[Partner_type] ([ID], [Type]) VALUES (4, N'ОАО')
SET IDENTITY_INSERT [dbo].[Partner_type] OFF
GO
SET IDENTITY_INSERT [dbo].[Partners] ON 

INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (1, 1, N'База Строитель', N'Иванова Александра Ивановна', N'aleksandraivanova@ml.ru', N'493 123 45 6', N'652050, Кемеровская область, город Юрга, ул. Лесная, 15', N'2222455179', 7)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (2, 2, N'Паркет 29', N'Петров Василий Петрович', N'vppetrov@vl.ru', N'987 123 56 7', N'164500, Архангельская область, город Северодвинск, ул. Строителей, 18', N'3333888520', 7)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (3, 3, N'Стройсервис', N'Соловьев Андрей Николаевич', N'ansolovev@st.ru', N'812 223 32 0', N'188910, Ленинградская область, город Приморск, ул. Парковая, 21', N'4440391035', 7)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (4, 4, N'Ремонт и отделка', N'Воробьева Екатерина Валерьевна', N'ekaterina.vorobeva@ml.ru', N'+79299172902', N'143960, Московская область, город Реутов, ул. Свободы, 51', N'1111520857', 8)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (11, 1, N'МонтажПро', N'Степанов Степан Сергеевич', N'stepanov@stepan.ru', N'+79128883333', N'309500, Белгородская область, город Старый Оскол, ул. Рабочая, 122', N'5552431140', 10)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (12, 1, N'ООО "БанкПроБанк"', N'Петров Петр Петрович', N'bankprobank@gmail.com', N'+7999999999 ', N'ул. Центральная, 32, Москва', N'3612783123', 10)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (13, 2, N'МонтажЛоу', N'Богданов Богдан Богданович', N'bogdanovich@mai.ru', N'+79666666666', N'ул, Центральная, 33, Москва', N'3462718934', 10)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (14, 2, N'вылоаоыврд', N'Тестов Тест Тестович', N'testovich@mail.ru', N'+78754221212', N'какая-то улица', N'3362178128', 10)
INSERT [dbo].[Partners] ([ID], [Type], [Name], [Director], [Email], [Number], [Address], [INN], [Rating]) VALUES (15, 1, N'тестович', N'Тестов Тест Тестович', N'testovich@yandex.ru', N'+79299172902', N'ул, Маркситская', N'3162783267', 10)
SET IDENTITY_INSERT [dbo].[Partners] OFF
GO
SET IDENTITY_INSERT [dbo].[Product_type] ON 

INSERT [dbo].[Product_type] ([ID], [Type], [RatioTypeProduct]) VALUES (1, N'Ламинат', 2.35)
INSERT [dbo].[Product_type] ([ID], [Type], [RatioTypeProduct]) VALUES (2, N'Массивная доска', 5.15)
INSERT [dbo].[Product_type] ([ID], [Type], [RatioTypeProduct]) VALUES (3, N'Паркетная доска', 4.34)
INSERT [dbo].[Product_type] ([ID], [Type], [RatioTypeProduct]) VALUES (4, N'Пробковое покрытие', 1.5)
SET IDENTITY_INSERT [dbo].[Product_type] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ID], [Type], [Name], [Number], [MinCost]) VALUES (1, 3, N'Паркетная доска Ясень темный однополосная 14 мм', N'8758385   ', CAST(4456.90 AS Decimal(10, 2)))
INSERT [dbo].[Products] ([ID], [Type], [Name], [Number], [MinCost]) VALUES (2, 3, N'Инженерная доска Дуб Французская елка однополосная 12 мм', N'8858958   ', CAST(7330.99 AS Decimal(10, 2)))
INSERT [dbo].[Products] ([ID], [Type], [Name], [Number], [MinCost]) VALUES (3, 1, N'Ламинат Дуб дымчато-белый 33 класс 12 мм', N'7750282   ', CAST(1799.33 AS Decimal(10, 2)))
INSERT [dbo].[Products] ([ID], [Type], [Name], [Number], [MinCost]) VALUES (4, 1, N'Ламинат Дуб серый 32 класс 8 мм с фаской', N'7028748   ', CAST(3890.41 AS Decimal(10, 2)))
INSERT [dbo].[Products] ([ID], [Type], [Name], [Number], [MinCost]) VALUES (5, 4, N'Пробковое напольное клеевое покрытие 32 класс 4 мм', N'5012543   ', CAST(5450.59 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
ALTER TABLE [dbo].[Partner_products]  WITH CHECK ADD  CONSTRAINT [FK__Partner_p__Partn__440B1D61] FOREIGN KEY([Partner])
REFERENCES [dbo].[Partners] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Partner_products] CHECK CONSTRAINT [FK__Partner_p__Partn__440B1D61]
GO
ALTER TABLE [dbo].[Partner_products]  WITH CHECK ADD FOREIGN KEY([Product])
REFERENCES [dbo].[Products] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Partners]  WITH CHECK ADD  CONSTRAINT [FK__Partners__Type__403A8C7D] FOREIGN KEY([Type])
REFERENCES [dbo].[Partner_type] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Partners] CHECK CONSTRAINT [FK__Partners__Type__403A8C7D]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([Type])
REFERENCES [dbo].[Product_type] ([ID])
ON DELETE CASCADE
GO
