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
CREATE TABLE invoice_details(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	PartyID INT NOT NULL FOREIGN KEY REFERENCES party(ID) ON DELETE CASCADE ,
	DateOfCreation DATE NOT NULL,
	GrandTotal INT NOT NULL
);
DROP TABLE invoice_details;

ALTER TABLE product_rate ADD CONSTRAINT PK_PRODUCT_RATE
PRIMARY KEY(ID);
DROP TABLE product_rate;
ALTER TABLE product_rate ADD PRIMARY KEY(ID);
CREATE TABLE invoice(
	ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Party_ID INT NOT NULL FOREIGN KEY REFERENCES party(ID) ON DELETE CASCADE,
	Product_ID INT NOT NULL FOREIGN KEY REFERENCES products(ID) ON DELETE CASCADE,
	Rate INT NOT NULL,
	Quantity INT NOT NULL,
	Invoice_ID INT NOT NULL
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

ALTER PROCEDURE sp_getall_product_rate AS
BEGIN
SELECT A.ID AS ID,B.Product_Name AS ProductName,A.Rate AS Rate,A.Date AS Date FROM product_rate AS A JOIN products AS B ON A.Product_ID = B.ID ORDER BY A.ID ASC; 
END;

ALTER PROCEDURE sp_getall_invoice AS
BEGIN
SELECT A.ID, B.Party_Name,A.DateOfCreation,A.GrandTotal  FROM invoice_details AS A INNER JOIN party AS B ON A.PartyID = B.ID 
ORDER BY B.Party_Name ASC, A.DateOfCreation DESC;
END;

ALTER PROCEDURE sp_getall_invoice_details @id INT AS
BEGIN
	SELECT A.ID,B.Party_Name,C.Product_Name,A.Rate,A.Quantity, A.Quantity * A.Rate AS Total FROM invoice AS A INNER JOIN party AS B 
	ON A.Party_ID = B.ID INNER JOIN products As C
	ON A.Product_ID = C.ID 
	WHERE A.Invoice_ID = @id;
END;

ALTER PROCEDURE sp_get_grandtotal @id INT AS
BEGIN
	UPDATE invoice_details SET GrandTotal = (SELECT Quantity*Rate FROM invoice WHERE Invoice_ID = @id) WHERE ID = @id;
	SELECT GrandTotal FROM invoice_details WHERE ID = @id;
END;

CREATE PROCEDURE sp_delete_invoice @id INT AS
BEGIN
	DELETE FROM invoice WHERE ID = @id;
END;

ALTER PROCEDURE sp_get_specific_invoice_detail @id INT AS
BEGIN
	SELECT A.ID,A.Party_ID,A.Product_ID,A.Rate,A.Quantity FROM invoice AS A INNER JOIN party AS B 
	ON A.Party_ID = B.ID INNER JOIN products As C
	ON A.Product_ID = C.ID
	WHERE A.ID = @id
END;

CREATE PROCEDURE sp_update_invoice_details @id INT, @productID INT, @rate INT, @qty INT  AS
BEGIN
	UPDATE invoice SET Product_ID = @productID, Rate = @rate, Quantity = @qty WHERE ID = @id;
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

CREATE PROCEDURE sp_store_product_rate @productID INT, @rate INT , @date DATE AS
BEGIN
	INSERT INTO product_rate VALUES(@productID,@rate,@date)
END;

CREATE PROCEDURE sp_store_invoice_details @partyID INT, @date DATE, @grandTotal INT AS
BEGIN
	INSERT INTO invoice_details VALUES(@partyID,@date,@grandTotal);
	SELECT SCOPE_IDENTITY();
END

ALTER PROCEDURE sp_store_invoice @partyID INT, @productID INT, @rate INT,@qty INT, @invoiceID INT AS
BEGIN
	INSERT INTO invoice VALUES(@partyID,@productID,@rate,@qty,@invoiceID);
	UPDATE invoice_details SET GrandTotal = (SELECT SUM(Quantity*Rate) FROM invoice WHERE Invoice_ID = @invoiceID) WHERE ID = @invoiceID;
END;

SELECT * FROM invoice_details WHERE ID = 7;

CREATE PROCEDURE sp_update_party @partyName VARCHAR(50), @partyID INT AS 
BEGIN
UPDATE party SET Party_Name = @partyName WHERE ID = @partyID;
END;

CREATE PROCEDURE sp_update_product @productName VARCHAR(50), @productID INT AS 
BEGIN
UPDATE products SET Product_Name = @productName WHERE ID = @productID;
END;

ALTER PROCEDURE sp_update_assign_party @assignPartyID INT, @productID INT, @partyID INT AS
BEGIN
UPDATE assign_party SET Product_ID = @productID, Party_ID = @partyID WHERE ID= @assignPartyID ;
END

CREATE PROCEDURE  sp_update_product_rate @rateID INT, @productID INT, @rate INT, @date VARCHAR(50) AS
BEGIN
UPDATE product_rate SET Product_ID = @productID, Rate = @rate, Date = @date WHERE ID = @rateID;
END;

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

CREATE PROCEDURE sp_check_product_rate_exists @productID INT, @rate INT, @date VARCHAR(50) AS
BEGIN
	IF EXISTS(SELECT * FROM product_rate WHERE Product_ID = @productID AND Rate = @rate AND Date = @date)
	BEGIN
		SELECT 'TRUE'
	END
	ELSE
	BEGIN
		SELECT 'FALSE'
	END

ALTER PROCEDURE sp_delete_party @partyID INT AS
BEGIN
		DELETE FROM party WHERE ID = @partyID
END;

ALTER PROCEDURE sp_delete_product @productID INT AS
BEGIN
		DELETE FROM products WHERE ID = @productId;
END;


		DELETE FROM products WHERE Product_Name 

ALTER PROCEDURE sp_getall_assign_party AS
BEGIN
	SELECT A.ID,B.Party_Name,C.Product_Name FROM assign_party AS A JOIN party AS B ON A.Party_ID = B.ID
	JOIN products AS C ON A.Product_ID = C.ID;
END;

CREATE PROCEDURE sp_delete_assign_party @id INT AS
BEGIN
	DELETE FROM assign_party WHERE ID = @id;
END;

CREATE PROCEDURE sp_delete_product_rate @id INT AS
BEGIN
	DELETE FROM product_rate WHERE ID = @id;
END;


ALTER PROCEDURE sp_get_party_assign_party AS
BEGIN
	SELECT DISTINCT A.Party_Name,A.ID FROM party AS A JOIN assign_party AS B
	ON A.ID = B.Party_ID ORDER BY A.ID;
END;

SELECT * FROM party ORDER BY ID;
SELECT * FROM assign_party ORDER BY ID;

ALTER PROCEDURE sp_get_product_per_party_assign_party @partyID INT AS
BEGIN
	SELECT B.Product_Name ,B.ID  FROM assign_party AS A 
	JOIN products AS B ON A.Product_ID = B.ID
	JOIN product_rate AS C ON A.Product_ID = C.Product_ID
	WHERE A.Party_ID = @partyID;
END;

ALTER PROCEDURE sp_get_party_from_assign_party AS
BEGIN
	SELECT A.ID, A.Party_Name FROM party AS A INNER JOIN assign_party AS B
	ON A.ID = B.Party_ID
END;

ALTER PROCEDURE sp_get_rate @id INT AS
BEGIN
	SELECT TOP 1 Rate FROM product_rate WHERE Product_ID = @id ORDER BY Date DESC;
END;



INSERT INTO party VALUES('xyz'),('Dhara');
INSERT INTO products VALUES('jaljira'),('biscuit');
INSERT INTO assign_party VALUES(3,1),(3,2),(4,3),(4,2);
INSERT INTO product_rate VALUES(1,20,CONVERT(date,'01-12-2024'));
INSERT INTO product_rate VALUES(2,25,CONVERT(date,'01-12-2024'));
INSERT INTO product_rate VALUES(3,56,CONVERT(date,'01-12-2024'));
SELECT * FROM party ORDER BY ID;
SELECT * FROM products ORDER BY ID;
SELECT * FROM assign_party ORDER BY ID;
SELECT * FROM product_rate ORDER BY ID;
SELECT * FROM invoice  ORDER BY ID;
SELECT * FROM invoice_details ORDER BY ID;
TRUNCATE TABLE invoice;
TRUNCATE TABLE invoice_details;
SELECT * FROM invoice WHERE Invoice_ID = 16;