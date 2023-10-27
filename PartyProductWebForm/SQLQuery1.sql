CREATE TABLE party(
	ID int NOT NULL IDENTITY(1,1),
	Party_Name VARCHAR(50) NOT NULL,
	PRIMARY KEY(ID)
);
ALTER TABLE party ADD UNIQUE(Party_Name);
CREATE TABLE products(
	ID int NOT NULL IDENTITY(1,1),
	Product_Name VARCHAR(50) NOT NULL,
	PRIMARY KEY(ID) 
);
ALTER TABLE products ADD UNIQUE(Product_Name);
CREATE NONCLUSTERED INDEX IX_NONCLUSTEREDINDEX_products ON products(Product_Name);
CREATE TABLE assign_party(
	ID INT NOT NULL IDENTITY(1,1),
	Party_ID INT NOT NULL FOREIGN KEY REFERENCES party(ID) ON DELETE CASCADE,
	Product_ID INT NOT NULL FOREIGN KEY REFERENCES products(ID) ON DELETE CASCADE,
	PRIMARY KEY(ID)
);
CREATE TABLE product_rate(
	ID INT NOT NULL IDENTITY(1,1),
	Product_ID INT NOT NULL FOREIGN KEY REFERENCES products(ID) ON DELETE CASCADE,
	Rate INT NOT NULL,
	Date DATE NOT NULL
);
DROP TABLE product_rate;
ALTER TABLE product_rate ADD PRIMARY KEY(ID);
CREATE TABLE invoice(
	ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Party_ID INT NOT NULL FOREIGN KEY REFERENCES party(ID) ON DELETE CASCADE,
	Product_ID INT NOT NULL FOREIGN KEY REFERENCES products(ID) ON DELETE CASCADE,
	Rate INT NOT NULL,
	Quantity INT NOT NULL
);
DROP TABLE invoice;
INSERT INTO party VALUES('Priyank'),('abc');
SELECT Party_Name FROM party;
SELECT Product_Name FROM products;

ALTER PROCEDURE sp_getall_party AS
BEGIN
SELECT * FROM party ORDER BY ID ASC;
END;

CREATE PROCEDURE sp_getall_products AS
BEGIN
SELECT * FROM products ORDER BY ID ASC;
END;

CREATE PROCEDURE sp_getall_assign_party AS
BEGIN
SELECT * FROM assign_party ORDER BY ID ASC;
END;

CREATE PROCEDURE sp_getall_product_rate AS
BEGIN
SELECT * FROM product_rate ORDER BY ID ASC
END;

CREATE PROCEDURE sp_getall_invoice AS
BEGIN
SELECT * FROM invoice ORDER BY ID ASC;
END;

CREATE PROCEDURE sp_store_party @partyName VARCHAR(50)  AS
BEGIN
INSERT INTO party VALUES (@partyName);
END;


CREATE PROCEDURE sp_store_product @productName VARCHAR(50)  AS
BEGIN
INSERT INTO products VALUES (@productName);
END;

CREATE PROCEDURE sp_store_assign_party @partyID INT, @productID INT AS
BEGIN
INSERT INTO assign_party VALUES(@partyID,@productID)
END;


CREATE PROCEDURE sp_update_party @partyName VARCHAR(50), @partyID INT AS 
BEGIN
UPDATE party SET Party_Name = @partyName WHERE ID = @partyID;
END;

CREATE PROCEDURE sp_update_product @productName VARCHAR(50), @productID INT AS 
BEGIN
UPDATE products SET Product_Name = @productName WHERE ID = @productID;
END;

CREATE PROCEDURE sp_update_assign_party @assignPartyID INT, @productID INT, @partyID INT AS
BEGIN
UPDATE assign_party SET Product_ID = @productID, Party_ID = @partyID WHERE ID= @assignPartyID ;
END

EXEC sp_update_assign_party @assignPartyID = 6, @partyID=12, @productID = 2;

CREATE PROCEDURE  sp_check_party_exists @partyName VARCHAR(50) AS
BEGIN
	IF EXISTS (SELECT * FROM party WHERE Party_Name = @partyName)
	BEGIN
		SELECT 'TRUE'
	END
	ELSE
	BEGIN
		SELECT 'FALSE'
	END
END;

CREATE PROCEDURE  sp_check_product_exists @productName VARCHAR(50) AS
BEGIN
	IF EXISTS (SELECT * FROM products WHERE Product_Name = @productName)
	BEGIN
		SELECT 'TRUE'
	END
	ELSE
	BEGIN
		SELECT 'FALSE'
	END
END;

CREATE PROCEDURE sp_check_assign_party_exists @partyID INT, @productID INT
AS
BEGIN
	IF EXISTS(SELECT * FROM assign_party WHERE Party_ID = @partyID AND Product_ID = @productID)
	BEGIN
		SELECT 'TRUE'
	END
	ELSE
	BEGIN
		SELECT 'FALSE'
	END
END;

CREATE PROCEDURE sp_delete_party @partyName VARCHAR(50) AS
BEGIN
		DELETE FROM party WHERE Party_Name = @partyName
END;

CREATE PROCEDURE sp_delete_product @productName VARCHAR(50) AS
BEGIN
		DELETE FROM products WHERE Product_Name = @productName
END;

ALTER PROCEDURE sp_getall_assign_party AS
BEGIN
	SELECT A.ID,B.Party_Name,C.Product_Name FROM assign_party AS A JOIN party AS B ON A.Party_ID = B.ID
	JOIN products AS C ON A.Product_ID = C.ID;
END;

CREATE PROCEDURE sp_delete_assign_party @id INT AS
BEGIN
	DELETE FROM assign_party WHERE ID = @id;
END;

CREATE PROCEDURE sp_get_party_assign_party AS
BEGIN
	SELECT Party_Name,ID FROM party;
END;

CREATE PROCEDURE sp_get_product_per_party_assign_party @partyID INT AS
BEGIN
	SELECT B.Product_Name AS ProductName FROM assign_party AS A 
	JOIN products AS B ON A.Product_ID = B.ID
	WHERE A.Party_ID = @partyID;
END;

INSERT INTO party VALUES('xyz'),('Dhara');
INSERT INTO products VALUES('jaljira'),('biscuit');
INSERT INTO assign_party VALUES(3,1),(3,2),(4,3),(4,2);
SELECT * FROM party ORDER BY ID;
SELECT * FROM products ORDER BY ID;
SELECT * FROM assign_party ORDER BY ID;
