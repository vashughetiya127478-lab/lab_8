using Microsoft.Data.Sqlite;

namespace DN_8.Data;

public static class DatabaseInitializer
{
    public static string ConnectionString =>
        $"Data Source={Path.Combine(AppContext.BaseDirectory, "inventory.db")}";

    public static void Initialize()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
CREATE TABLE IF NOT EXISTS Inventory (
    ItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    ItemName TEXT NOT NULL,
    Category TEXT NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice NUMERIC NOT NULL,
    Supplier TEXT NULL
);";
        command.ExecuteNonQuery();

        command.CommandText = "SELECT COUNT(*) FROM Inventory;";
        if (Convert.ToInt32(command.ExecuteScalar()) == 0)
        {
            command.CommandText = @"
INSERT INTO Inventory (ItemName, Category, Quantity, UnitPrice, Supplier) VALUES
('Keyboard', 'Electronics', 25, 650.00, 'ABC Suppliers'),
('Notebook', 'Stationery', 100, 45.00, 'Raj Stationers'),
('Mouse', 'Electronics', 40, 350.00, 'Tech Distributors');";
            command.ExecuteNonQuery();
        }
    }
}
