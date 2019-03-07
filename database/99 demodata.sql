USE Watches;
GO
SET NOCOUNT ON;
GO

-- Create movements
INSERT INTO dbo.Movement (Title) VALUES ('Eco-Drive'), ('Automatic'), ('Hand Wind'), ('Quartz');

DECLARE
	@Movement_EcoDrive INTEGER = (SELECT Id FROM dbo.Movement WHERE Title = 'Eco-Drive')
	, @Movement_Automatic INTEGER = (SELECT Id FROM dbo.Movement WHERE Title = 'Automatic')
	, @Movement_HandWind INTEGER = (SELECT Id FROM dbo.Movement WHERE Title = 'Hand Wind')
	, @Movement_Quartz INTEGER = (SELECT Id FROM dbo.Movement WHERE Title = 'Quartz');

-- Create brands
INSERT INTO dbo.Brand (Title, YearFounded, Description, DateCreated) 
VALUES
	('Citizen', 1918, 'Citizen Watch is an electronics company primarily known for its watches, and is the core company of a Japanese global corporate group based in Tokyo. In addition to Citizen brand watches, it is the parent of American watch company Bulova, and is also known for manufacturing small electronics such as calculators.', GETDATE()),
	('Omega', 1903, 'Omega SA is a Swiss luxury watchmaker based in Biel/Bienne, Switzerland.', GETDATE()),
	('Seiko', 1881, 'Seiko Holdings Corporation is a Japanese holding company that has subsidiaries which manufactures and sells watches, clocks, electronic devices, semiconductors, jewelries, and optical products.', GETDATE()),
	('Rolex', 1915, 'Rolex SA is a Swiss luxury watch manufacturer based in Geneva, Switzerland.', GETDATE()),
	('Hamilton', 1892, 'The Hamilton Watch Company is a Swiss manufacturer of wristwatches based in Bienne, Switzerland.', GETDATE()),
	('Timex', 1854, 'Timex is an American manufacturing company founded in 1854.', GETDATE());

DECLARE
	@Brand_Citizen INTEGER = (SELECT Id FROM dbo.Brand WHERE Title = 'Citizen')
	, @Brand_Omega INTEGER = (SELECT Id FROM dbo.Brand WHERE Title = 'Omega')
	, @Brand_Seiko INTEGER = (SELECT Id FROM dbo.Brand WHERE Title = 'Seiko')
	, @Brand_Rolex INTEGER = (SELECT Id FROM dbo.Brand WHERE Title = 'Rolex')
	, @Brand_Hamilton INTEGER = (SELECT Id FROM dbo.Brand WHERE Title = 'Hamilton')
	, @Brand_Timex INTEGER = (SELECT Id FROM dbo.Brand WHERE Title = 'Timex');

-- Genders
DECLARE
	@Unisex INT = 0
	, @Mens INT = 1
	, @Ladies INT = 2;

-- Case Materials
DECLARE
	@StainlessSteel INT = 0
	, @Bronze INT = 1
	, @Titanium INT = 2
	, @Gold INT = 3
	, @Brass INT = 4

-- Create watches
INSERT INTO dbo.Watch(Model, Title, Gender, CaseSize, CaseMaterial, DateCreated, BrandId, MovementId)
VALUES
	('AW2020-82L', 'Titanium Eco-Drive', @Mens, 41, @Titanium, GETDATE(), @Brand_Citizen, @Movement_EcoDrive)
	, ('SNZG15', '5 Sport Automatic', @Mens, 41, @StainlessSteel, GETDATE(), @Brand_Seiko, @Movement_Automatic)
	, ('114060', 'Submariner', @Mens, 40, @StainlessSteel, GETDATE(), @Brand_Rolex, @Movement_Automatic)
	, ('214270', 'Explorer', @Mens, 39, @StainlessSteel, GETDATE(), @Brand_Rolex, @Movement_Automatic)
	, ('H68201943', 'Khaki Field Blue Dial', @Mens, 38, @StainlessSteel, GETDATE(), @Brand_Hamilton, @Movement_Quartz)
	, ('TW2R47500D7PF', 'Allied Chronograph', @Mens, 42, @Brass, GETDATE(), @Brand_Timex, @Movement_Quartz)
