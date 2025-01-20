USE master;

DROP DATABASE IF EXISTS TrivialBrickDB;
CREATE DATABASE TrivialBrickDB;
GO
USE TrivialBrickDB;

-- Creating the Users table
CREATE TABLE users (
    ID INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    mail NVARCHAR(100) UNIQUE NOT NULL,
    password NVARCHAR(100) NOT NULL
);

-- Creating the Admins table
CREATE TABLE admins (
    user_id INT PRIMARY KEY,
    FOREIGN KEY (user_id) REFERENCES users(ID) ON DELETE CASCADE
);

-- Creating the Clients table
CREATE TABLE clients (
    user_id INT PRIMARY KEY,
    nif INT,
    FOREIGN KEY (user_id) REFERENCES users(ID) ON DELETE CASCADE
);

-- Creating the Product table
CREATE TABLE products (
    model INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(7,2) NOT NULL,
    description TEXT,
    image VARCHAR(255),
);

-- Creating the Part table
CREATE TABLE parts (
    part_id INT PRIMARY KEY,
    image VARCHAR(255),
    stock INT NOT NULL,
);

-- Creating the ProductPart table
CREATE TABLE products_parts (
    part_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    PRIMARY KEY (part_id, product_id),
    FOREIGN KEY (part_id) REFERENCES parts(part_id) ON DELETE NO ACTION,
    FOREIGN KEY (product_id) REFERENCES products(model) ON DELETE CASCADE
);

-- Creating the Instruction table
CREATE TABLE instructions (
    seq_num INT NOT NULL,
    product_id INT NOT NULL,
    image VARCHAR(255),
    qnt_parts INT NOT NULL,
    PRIMARY KEY (seq_num, product_id),
    FOREIGN KEY (product_id) REFERENCES products(model) ON DELETE CASCADE
);

-- Creating the Order table
CREATE TABLE orders (
    order_id INT PRIMARY KEY IDENTITY(1,1),
    address VARCHAR(150) NOT NULL,
    state VARCHAR(15) NOT NULL CHECK (state IN ('wait_line', 'assembly_line', 'finished')),
    product_id INT NOT NULL,
    client_id INT NOT NULL,
    price DECIMAL(7,2) NOT NULL,
    date DATE NOT NULL,
    FOREIGN KEY (product_id) REFERENCES products(model) ON DELETE NO ACTION,
    FOREIGN KEY (client_id) REFERENCES clients(user_id) ON DELETE NO ACTION
);

-- Creating the Invoice table
CREATE TABLE invoices (
    invoice_id INT PRIMARY KEY IDENTITY(1,1),
    datetime DATETIME NOT NULL,
    client_id INT NOT NULL,
    order_id INT NOT NULL,
    FOREIGN KEY (client_id) REFERENCES clients(user_id) ON DELETE NO ACTION,
    FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE NO ACTION
);

-- Creating the AssemblyLine table
CREATE TABLE assembly_lines (
    assembly_line_id INT PRIMARY KEY,
    state VARCHAR(20) NOT NULL CHECK (state IN ('active', 'inactive')),
    order_id INT,
    FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE SET NULL
);

-- Creating the Notification table
CREATE TABLE notifications (
    notification_id INT PRIMARY KEY IDENTITY(1,1),
    message TEXT NOT NULL,
    datetime DATETIME NOT NULL,
    client_id INT NOT NULL,
    order_id INT NOT NULL,
    FOREIGN KEY (client_id) REFERENCES clients(user_id) ON DELETE CASCADE,
    FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE
);

-- Creating Admin
INSERT INTO Users (name, mail, password) VALUES ('admin', 'admin@TrivialBrick.pt', 'admin');
INSERT INTO Admins (user_id) SELECT ID FROM Users WHERE name = 'admin';

GO
-- Print message to terminal
PRINT 'Everything is ready!';
