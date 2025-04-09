using Microsoft.EntityFrameworkCore;
using WebStore.Entities;

namespace WebStore.Assignments
{
    /// Additional tutorial materials https://dotnettutorials.net/lesson/linq-to-entities-in-entity-framework-core/

    /// <summary>
    /// This class demonstrates various LINQ query tasks 
    /// to practice querying an EF Core database.
    /// 
    /// ASSIGNMENT INSTRUCTIONS:
    ///   1. For each method labeled "TODO", write the necessary
    ///      LINQ query to return or display the required data.
    ///      
    ///   2. Print meaningful output to the console (or return
    ///      collections, as needed).
    ///      
    ///   3. Test each method by calling it from your Program.cs
    ///      or test harness.
    /// </summary>
    public class LinqQueriesAssignment
    {

       

        private readonly ThirdAssignmentContext _dbContext;

        public LinqQueriesAssignment(ThirdAssignmentContext context)
        {
            _dbContext = context;
        }


        /// <summary>
        /// 1. List all customers in the database:
        ///    - Print each customer's full name (First + Last) and Email.
        /// </summary>
        public async Task Task01_ListAllCustomers()
        {
            var customers = await _dbContext.Customers
               // .AsNoTracking() // optional for read-only
               .ToListAsync();

            Console.WriteLine("=== TASK 01: List All Customers ===");

            foreach (var c in customers)
            {
                Console.WriteLine($"{c.FirstName} {c.LastName} - {c.Email}");
            }
        }

        /// <summary>
        /// 2. Fetch all orders along with:
        ///    - Customer Name
        ///    - Order ID
        ///    - Order Status
        ///    - Number of items in each order (the sum of OrderItems.Quantity)
        /// </summary>
        public async Task Task02_ListOrdersWithItemCount()
        {
            var orders = await _dbContext.Orders
                .Select( o =>new
                {
                    CustomerName= o.Customer.FirstName + ' ' + o.Customer.LastName,
                    OrderId = o.OrderId,
                    OrderStatus = o.OrderStatus,
                    ItemCount = o.OrderItems.Sum(oi => oi.Quantity)
                })
                .ToListAsync();

            Console.WriteLine(" ");
            Console.WriteLine("=== TASK 02: List Orders With Item Count ===");

            foreach (var order in orders)
            {
                Console.WriteLine($"Customer: {order.CustomerName}, Order ID: {order.OrderId},  Status: {order.OrderStatus}, Item Count: {order.ItemCount}");
            }
        }

        /// <summary>
        /// 3. List all products (ProductName, Price),
        ///    sorted by price descending (highest first).
        /// </summary>
        public async Task Task03_ListProductsByDescendingPrice()
        {
            var products = await _dbContext.Products
                .OrderByDescending(p => p.Price)
                .Select(p => new
                {
                    ProductName = p.ProductName,
                    Price = p.Price
                })
                .ToListAsync();

            Console.WriteLine(" ");
            Console.WriteLine("=== Task 03: List Products By Descending Price ===");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductName} - {product.Price:C}");
            }
        }

        /// <summary>
        /// 4. Find all "Pending" orders (order status = "Pending")
        ///    and display:
        ///      - Customer Name
        ///      - Order ID
        ///      - Order Date
        ///      - Total price (sum of unit_price * quantity - discount) for each order
        /// </summary>
        public async Task Task04_ListPendingOrdersWithTotalPrice()
        {
            var pendingOrders = await _dbContext.Orders
                .Where(o => o.OrderStatus == "Pending") 
                .Select(o => new
                {
                    CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.OrderItems
                        .Sum(oi => (oi.UnitPrice * oi.Quantity) - oi.Discount) // Вычисление общей стоимости
                })
                .ToListAsync();

            Console.WriteLine(" ");
            Console.WriteLine("=== Task 04: List Pending Orders With Total Price ===");
            foreach (var order in pendingOrders)
            {
                Console.WriteLine($"Customer: {order.CustomerName}, Order ID: {order.OrderId}, " +
                                  $"Order Date: {order.OrderDate}, Total Price: {order.TotalPrice:C}");
            }
        }

        /// <summary>
        /// 5. List the total number of orders each customer has placed.
        ///    Output should show:
        ///      - Customer Full Name
        ///      - Number of Orders
        /// </summary>
        public async Task Task05_OrderCountPerCustomer()
        {
            var result = await _dbContext.Customers
                .Select(c => new
                {
                    CustomerFullName = c.FirstName + " " + c.LastName,
                    OrderCount = c.Orders.Count
                })
                .ToListAsync();

            Console.WriteLine(" ");
            Console.WriteLine("=== Task 05: Order Count Per Customer ===");

            foreach (var item in result)
                {
                    Console.WriteLine($"Customer: {item.CustomerFullName}, Orders: {item.OrderCount}");
                }
        }

        /// <summary>
        /// 6. Show the top 3 customers who have placed the highest total order value overall.
        ///    - For each customer, calculate SUM of (OrderItems * Price).
        ///      Then pick the top 3.
        /// </summary>
        public async Task Task06_Top3CustomersByOrderValue()
        {
            var result = await _dbContext.Orders
                .GroupBy(o => o.Customer)
                .Select(g => new
                {
                    Customer = g.Key,
                    TotalValue = g.Sum(o => o.OrderItems.Sum(oi => (oi.UnitPrice * oi.Quantity) - oi.Discount))
                })
                .OrderByDescending(x => x.TotalValue)
                .Take(3)
                .ToListAsync();

            Console.WriteLine(" ");
            Console.WriteLine("=== Task 06: Top 3 Customers By Order Value ===");

            foreach (var item in result)
            {
                Console.WriteLine($"Customer: {item.Customer.FirstName} {item.Customer.LastName}, Total Order Value: {item.TotalValue}");
            }

        }
        

        /// <summary>
        /// 7. Show all orders placed in the last 30 days (relative to now).
        ///    - Display order ID, date, and customer name.
        /// </summary>
        public async Task Task07_RecentOrders()
        {
            var result = await _dbContext.Orders
                .Where(o => o.OrderDate >= DateTime.Now.AddDays(-30))
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CustomerName = o.Customer.FirstName + " " + o.Customer.LastName
                })
                .ToListAsync();

            
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 07: Recent Orders ===");

            foreach (var item in result)
            {
                Console.WriteLine($"Order ID: {item.OrderId}, Date: {item.OrderDate}, Customer: {item.CustomerName}");
            }   
        }

        /// <summary>
        /// 8. For each product, display how many total items have been sold
        ///    across all orders.
        ///    - Product name, total sold quantity.
        ///    - Sort by total sold descending.
        /// </summary>
        public async Task Task08_TotalSoldPerProduct()
        {
        var result = await _dbContext.OrderItems
                .GroupBy(oi => oi.Product)
                .Select(g => new
                {
                    ProductName = g.Key.ProductName,
                    TotalSold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .ToListAsync();
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 08: Total Sold Per Product ===");

            foreach (var item in result)
            {
                Console.WriteLine($"Product: {item.ProductName}, Total Sold: {item.TotalSold}");
            }
        }

        /// <summary>
        /// 9. List any orders that have at least one OrderItem with a Discount > 0.
        ///    - Show Order ID, Customer name, and which products were discounted.
        /// </summary>
        public async Task Task09_DiscountedOrders()
        {
        var result = await _dbContext.Orders
                .Where(o => o.OrderItems.Any(oi => oi.Discount > 0))
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                    DiscountedProducts = o.OrderItems.Where(oi => oi.Discount > 0).Select(oi => oi.Product.ProductName)
                })
                .ToListAsync();


            Console.WriteLine(" ");
            Console.WriteLine("=== Task 09: Discounted Orders ===");

            foreach (var item in result)
            {
                Console.WriteLine($"Order ID: {item.OrderId}, Customer: {item.CustomerName}");
                foreach (var product in item.DiscountedProducts)
                {
                    Console.WriteLine($"  Discounted Product: {product}");
                }
    }
        }

        /// <summary>
        /// 10. (Open-ended) Combine multiple joins or navigation properties
        ///     to retrieve a more complex set of data. For example:
        ///     - All orders that contain products in a certain category
        ///       (e.g., "Electronics"), including the store where each product
        ///       is stocked most. (Requires `Stocks`, `Store`, `ProductCategory`, etc.)
        ///     - Or any custom scenario that spans multiple tables.
        /// </summary>
        public async Task Task10_AdvancedQueryExample()
        {
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 10: Advanced Query Example ===");
            var result = await _dbContext.Products
                .Join(_dbContext.ProductCategories,
                    p => p.ProductId,
                    pc => pc.ProductId,
                    (p, pc) => new { Product = p, ProductCategory = pc })
                .Join(_dbContext.Categories,
                    combined => combined.ProductCategory.CategoryId,
                    c => c.CategoryId,
                    (combined, c) => new { combined.Product, Category = c })
                .Where(x => x.Category.CategoryName == "Electronics")
                .Join(_dbContext.OrderItems,
                    combined => combined.Product.ProductId,
                    oi => oi.ProductId,
                    (combined, oi) => new { combined.Product, OrderItem = oi })
                .Join(_dbContext.Orders,
                    combined => combined.OrderItem.OrderId,
                    o => o.OrderId,
                    (combined, o) => new { combined.Product, Order = o })
                .Join(_dbContext.Stocks,
                    combined => combined.Product.ProductId,
                    s => s.ProductId,
                    (combined, s) => new { combined.Product, combined.Order, Stock = s })
                .GroupBy(x => new { x.Product.ProductName, x.Stock.Store.StoreName })
                .Select(g => new
                {
                    ProductName = g.Key.ProductName,
                    StoreName = g.Key.StoreName,
                    MaxStock = g.Max(x => x.Stock.QuantityInStock)
                })
                .OrderByDescending(x => x.MaxStock)
                .ToListAsync();

            foreach (var item in result)
            {
                Console.WriteLine($"Product: {item.ProductName}, Store: {item.StoreName}, Max Stock: {item.MaxStock}");
            }
        }
        
    }
}
