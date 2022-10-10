CREATE PROCEDURE pr_GetOrderSummary (@StartDate DATETIME, @EndDate DATETIME, @CustomerID nvarchar(5) NULL, @EmployeeID int NULL)
AS

WITH ORDER_VALUE AS
(
	SELECT 
		OD.OrderID,
		ROUND( SUM(OD.UnitPrice * OD.Quantity - (OD.UnitPrice * OD.Quantity * OD.Discount)), 2) AS TotalOrderValue,
		COUNT(OD.ProductID) AS NumberOfDifferentProducts,
		--SUM(O.Freight) -- IF FREIGHT COST IS PRICED BY PRODUCT INSTEAD OF ORDER (REMOVE FROM GROUP BY IF USING THIS CALCULATION)
		O.Freight,
		COUNT(DISTINCT O.OrderID) AS NumberOfOrders,
		O.OrderDate
	FROM [Order Details] OD
	LEFT JOIN Orders O
		ON O.OrderID = OD.OrderID
	GROUP BY OD.OrderID, O.Freight, O.OrderDate
)

SELECT 
	OV.OrderID,
	CONCAT(E.TitleOfCourtesy, E.FirstName, E.LastName) AS EmployeeFullName,
	S.CompanyName,
	C.CompanyName,
	OV.NumberOfOrders,
	OV.OrderDate,
	OV.Freight,
	OV.NumberOfDifferentProducts,
	OV.TotalOrderValue
FROM ORDER_VALUE OV
LEFT JOIN Orders O 
	ON OV.OrderID = O.OrderID
LEFT JOIN Shippers S
	ON O.ShipVia = S.ShipperID
LEFT JOIN Customers C
	ON O.CustomerID = C.CustomerID
LEFT JOIN Employees E
	ON O.EmployeeID = E.EmployeeID

WHERE 
	OV.OrderDate >= @StartDate
	AND OV.OrderDate <= @EndDate
	AND C.CustomerID = @CustomerID
	AND E.EmployeeID = @EmployeeID