CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Role NVARCHAR(50) NOT NULL,
    Address NVARCHAR(255),
    PhoneNumber NVARCHAR(15)
);

CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    OrderDate DATE NOT NULL,
    TotalPrice FLOAT NOT NULL,
    DeliveryAddress NVARCHAR(255) NOT NULL,
    Status VARCHAR(50)
);

CREATE TABLE Restaurants (
    RestaurantId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(15),
    Rating FLOAT
);

CREATE TABLE MenuItems (
    MenuItemId INT PRIMARY KEY IDENTITY(1,1),
    RestaurantId INT FOREIGN KEY REFERENCES Restaurants(RestaurantId),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Price FLOAT NOT NULL
);

CREATE TABLE Cuisines (
    CuisineId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE RestaurantCuisines (
    RestaurantCuisineId INT PRIMARY KEY IDENTITY(1,1),
    RestaurantId INT FOREIGN KEY REFERENCES Restaurants(RestaurantId),
    CuisineId INT FOREIGN KEY REFERENCES Cuisines(CuisineId)
);

CREATE TABLE Reviews (
    ReviewId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    RestaurantId INT FOREIGN KEY REFERENCES Restaurants(RestaurantId),
    Rating FLOAT NOT NULL,
    Comment NVARCHAR(MAX),
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId)
);

CREATE TABLE OrderDetails (
    OrderDetailsId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    MenuItemId INT FOREIGN KEY REFERENCES MenuItems(MenuItemId),
    Quantity INT NOT NULL
);

CREATE TABLE Promotions (
    PromotionId INT PRIMARY KEY IDENTITY(1,1),
    RestaurantId INT FOREIGN KEY REFERENCES Restaurants(RestaurantId),
    PromoCode NVARCHAR(50) NOT NULL,
    DiscountPercentage FLOAT NOT NULL
);

CREATE TABLE PaymentMethods (
    PaymentMethodId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    CardName NVARCHAR(100) NOT NULL,
    CardNumber NVARCHAR(19) NOT NULL,
    ExpiryDate DATE NOT NULL
);

CREATE TABLE CartItems (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    MenuItemId INT FOREIGN KEY REFERENCES MenuItems(MenuItemId),
    Quantity INT NOT NULL
);
