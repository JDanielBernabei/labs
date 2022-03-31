using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Sopra.Labs.ConsoleApp3.Models;

namespace Sopra.Labs.ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Ejercicios31032022();
        }

        static void Ejercicios31032022()
        {
            var context = new ModelNorthwind();

            // Clientes de USA
            var clientsUSA = context.Customers
                    .Where(r => r.Country.Contains("USA"))
                    .ToList();

            foreach (var client in clientsUSA) Console.WriteLine($"CompanyName: {client.CompanyName} - Country: {client.Country}");
            Console.ReadKey();
            Console.WriteLine();

            // Proveedores (Suppliers) de BERLIN
            var suppliersBerlin = context.Suppliers
                    .Where(r => r.City.Contains("Berlin"))
                    .ToList();

            foreach (var supplier in suppliersBerlin) Console.WriteLine($"CompanyName: {supplier.CompanyName} - ContactName: {supplier.ContactName} - Ciudad: {supplier.City}");
            Console.ReadKey();
            Console.WriteLine();

            // Los empleados con ID 3, 5 y 8
            var employees = context.Employees
                    .Where(r => r.EmployeeID == 3 || r.EmployeeID == 5 || r.EmployeeID == 8)
                    .ToList();
            /*
            int[] employeesIds = new int[] {3,5,8};

            var r3a = context.Employees
                    .Where(r => new int[] {3,5,8}.Contains(r.EmployeeID))
                    .ToList();

            var r3b = context.Employees
                    .Where(r => employeesIds.Contains(r.EmployeeID))
                    .ToList();
             */

            foreach (var employee in employees) Console.WriteLine($"ID: {employee.EmployeeID} - FirstName: {employee.FirstName} - LastName: {employee.LastName}");
            Console.ReadKey();
            Console.WriteLine();

            // Productos con stock > 0
            var productsInStock = context.Products
                    .Where(r => r.UnitsInStock > 0)
                    .ToList();

            foreach (var product in productsInStock) Console.WriteLine($"Name: {product.ProductName} - Stock: {product.UnitsInStock}");
            Console.ReadKey();
            Console.WriteLine();

            // Productos con stock > 0 de los proveedores con ID 1, 3, y 5
            var productsInStockProvider = context.Products
                    .Where(r => r.UnitsInStock > 0 && (r.SupplierID == 1 || r.SupplierID == 3 || r.SupplierID == 5))
                    .ToList();

            foreach (var product in productsInStockProvider) Console.WriteLine($"Name: {product.ProductName} - Stock: {product.UnitsInStock} - SupplierID: {product.SupplierID}");
            Console.ReadKey();
            Console.WriteLine();

            // Productos precio > 20 y < 90
            var produtsOkPrice = context.Products
                    .Where(r => r.UnitPrice > 20 && r.UnitPrice < 90)
                    .ToList();

            foreach (var product in produtsOkPrice) Console.WriteLine($"Name: {product.ProductName} - Price: {product.UnitPrice}");
            Console.ReadKey();
            Console.WriteLine();

            // Pedidos entre 01.01.97 y 15.07.97
            var ordersFrom1997 = context.Orders
                    .Where(r => r.OrderDate >= new DateTime(1997, 1, 1) && r.OrderDate <= new DateTime(1997, 7, 15))
                    .ToList();

            foreach (var order in ordersFrom1997) Console.WriteLine($"ID: {order.OrderID} - Date: {order.OrderDate}");
            Console.ReadKey();
            Console.WriteLine();

            // Pedidos del 97 registrado por los empleados con id 1, 3, 4 y 8
            var ordersFrom1997Employee = context.Orders
                    .Where(r => (r.EmployeeID == 1 || r.EmployeeID == 3 || r.EmployeeID == 4 || r.EmployeeID == 8)
                                && r.OrderDate >= new DateTime(1997, 1, 1) && r.OrderDate < new DateTime(1998, 1, 1))
                    .ToList();

            foreach (var order in ordersFrom1997Employee) Console.WriteLine($"ID: {order.OrderID} - Date: {order.OrderDate} - EmployeeID: {order.EmployeeID}");
            Console.ReadKey();
            Console.WriteLine();

            // Pedidos de abril del 96
            var ordersFrom199704 = context.Orders
                    .Where(r => r.OrderDate >= new DateTime(1996, 04, 1) && r.OrderDate < new DateTime(1996, 05, 1))
                    .ToList();

            foreach (var order in ordersFrom199704) Console.WriteLine($"ID: {order.OrderID} - date: {order.OrderDate}");
            Console.ReadKey();
            Console.WriteLine();

            // Pedidos realizados el dia 1 de cada mes en el año 98
            var ordersOn1st = context.Orders
                    .Where(r => r.OrderDate != null && r.OrderDate.Value.Year == 1998 && r.OrderDate.Value.Day == 1)
                    .ToList();

            foreach (var order in ordersOn1st) Console.WriteLine($"ID: {order.OrderID} - date: {order.OrderDate}");
            Console.ReadKey();
            Console.WriteLine();

            // Clientes que no tienen fax
            var clientsNoFax = context.Customers
                    .Where(r => r.Fax == null)
                    .ToList();

            foreach (var client in clientsNoFax) Console.WriteLine($"CompanyName: {client.CompanyName} - ContactName: {client.ContactName} - Fax: {client.Fax}");
            Console.ReadKey();
            Console.WriteLine();

            // Los 10 productos mas baratos
            var cheapProducts = context.Products
                    .OrderBy(r => r.UnitPrice)
                    .Take(10)
                    .ToList();

            foreach (var product in cheapProducts) Console.WriteLine($"Name: {product.ProductName} - Price: {product.UnitPrice}");
            Console.ReadKey();
            Console.WriteLine();

            // Los 10 productos mas caros con stock
            var expensiveProductsStock = context.Products
                    .Where(r => r.UnitsInStock > 0)
                    .OrderByDescending(r => r.UnitPrice)
                    .Take(10)
                    .ToList();

            foreach (var product in expensiveProductsStock) Console.WriteLine($"Name: {product.ProductName} - Price: {product.UnitPrice} - Stock: {product.UnitsInStock}");
            Console.ReadKey();
            Console.WriteLine();

            // Empresas de la letra B de UK
            var companyUKStartB = context.Customers
                    .Where(r => r.CompanyName.StartsWith("B") && r.Country == "UK")
                    .ToList();

            foreach (var client in companyUKStartB) Console.WriteLine($"CompanyName: {client.CompanyName}");
            Console.ReadKey();
            Console.WriteLine();

            // Productos de la categoria 3 y 5
            var prductsCategory = context.Products
                    .Where(r => r.CategoryID == 3 || r.CategoryID == 5)
                    .ToList();

            foreach (var product in prductsCategory) Console.WriteLine($"Name: {product.ProductName} - CategoryID: {product.CategoryID}");
            Console.ReadKey();
            Console.WriteLine();

            // Valor total del stock
            var totalStock = context.Products
                    .Sum(r => r.UnitsInStock * r.UnitPrice);

            Console.WriteLine($"Stock total: {totalStock}");
            Console.WriteLine();

            // Todos los pedidos de los clientes en Argentina
            var ordersInArgentina = context.Orders_Qries
                .Where(r => r.Country.Contains("Argentina"))
                .ToList();

            /*
             var r16 = CUSTOMERID == ARGENTINA
            var r17 ORDERS
                WHERE r16.Contains(r.CustomerID)
             */

            foreach (var order in ordersInArgentina) Console.WriteLine($"ID: {order.OrderID} - Country: {order.Country}");
            Console.WriteLine();
        }
    }
}
