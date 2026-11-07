using DN_8.Models;
using Microsoft.Data.Sqlite;

namespace DN_8.Data;

public class InventoryRepository
{
    private readonly string _connectionString = DatabaseInitializer.ConnectionString;

    public List<InventoryItem> GetAll()
    {
        var items = new List<InventoryItem>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT ItemId, ItemName, Category, Quantity, UnitPrice, Supplier FROM Inventory ORDER BY ItemId;";
        using var reader = command.ExecuteReader();
        while (reader.Read()) items.Add(Map(reader));
        return items;
    }

    public InventoryItem? GetById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT ItemId, ItemName, Category, Quantity, UnitPrice, Supplier FROM Inventory WHERE ItemId = $id;";
        command.Parameters.AddWithValue("$id", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public void Add(InventoryItem item)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Inventory (ItemName, Category, Quantity, UnitPrice, Supplier)
VALUES ($name, $category, $quantity, $price, $supplier);";
        AddParameters(command, item);
        command.ExecuteNonQuery();
    }

    public bool Update(InventoryItem item)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"UPDATE Inventory SET ItemName=$name, Category=$category, Quantity=$quantity,
UnitPrice=$price, Supplier=$supplier WHERE ItemId=$id;";
        AddParameters(command, item);
        command.Parameters.AddWithValue("$id", item.ItemId);
        return command.ExecuteNonQuery() == 1;
    }

    public bool Delete(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Inventory WHERE ItemId=$id;";
        command.Parameters.AddWithValue("$id", id);
        return command.ExecuteNonQuery() == 1;
    }

    private static void AddParameters(SqliteCommand command, InventoryItem item)
    {
        command.Parameters.AddWithValue("$name", item.ItemName);
        command.Parameters.AddWithValue("$category", item.Category);
        command.Parameters.AddWithValue("$quantity", item.Quantity);
        command.Parameters.AddWithValue("$price", item.UnitPrice);
        command.Parameters.AddWithValue("$supplier", (object?)item.Supplier ?? DBNull.Value);
    }

    private static InventoryItem Map(SqliteDataReader reader) => new()
    {
        ItemId = reader.GetInt32(0),
        ItemName = reader.GetString(1),
        Category = reader.GetString(2),
        Quantity = reader.GetInt32(3),
        UnitPrice = Convert.ToDecimal(reader.GetValue(4)),
        Supplier = reader.IsDBNull(5) ? null : reader.GetString(5)
    };
}
