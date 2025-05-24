-- Create the Transactions table
CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    TradeID INT NOT NULL,
    Version INT NOT NULL,
    SecurityCode VARCHAR(10) NOT NULL,
    Quantity INT NOT NULL,
    Action VARCHAR(10) NOT NULL,  -- INSERT, UPDATE, CANCEL
    BuySell VARCHAR(10) NOT NULL  -- BUY, SELL
);

-- Insert sample test data
INSERT INTO Transactions (TradeID, Version, SecurityCode, Quantity, Action, BuySell) VALUES
(1, 1, 'REL', 50, 'INSERT', 'Buy'),
(2, 1, 'ITC', 40, 'INSERT', 'Buy'),
(3, 1, 'INF', 70, 'INSERT', 'Sell'),
(1, 2, 'REL', 60, 'INSERT', 'Buy'),
(2, 2, 'ITC', 30, 'UPDATE', 'Buy'),
(4, 1, 'INF', 20, 'CANCEL', 'Buy');

select * from Transactions;