using System;
using System.Collections.Generic;
using System.Linq;

namespace SportPitShop
{
    public class CartItem
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public int UnitPriceRUB { get; set; }
        public int DiscountPct { get; set; }
        public int Qty { get; set; }

        public int FinalUnitPrice => (UnitPriceRUB * (100 - DiscountPct)) / 100;
        public int LineTotal => FinalUnitPrice * Qty;
    }

    public static class Cart
    {
        private static readonly Dictionary<string, CartItem> _items = new Dictionary<string, CartItem>(StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyCollection<CartItem> Items => _items.Values.ToList().AsReadOnly();

        public static int Count => _items.Values.Sum(i => i.Qty);
        public static int TotalRUB => _items.Values.Sum(i => i.LineTotal);

        public static void Clear() => _items.Clear();

        public static void Add(string sku, string name, int unitPrice, int discountPct, int qty = 1)
        {
            if (string.IsNullOrWhiteSpace(sku)) return;
            if (qty <= 0) qty = 1;

            if (_items.TryGetValue(sku, out var it))
            {
                it.Qty += qty;
                return;
            }

            _items[sku] = new CartItem
            {
                SKU = sku,
                Name = name ?? "",
                UnitPriceRUB = unitPrice,
                DiscountPct = discountPct,
                Qty = qty
            };
        }

        public static void Remove(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) return;
            _items.Remove(sku);
        }

        public static void SetQty(string sku, int qty)
        {
            if (string.IsNullOrWhiteSpace(sku)) return;
            if (!_items.TryGetValue(sku, out var it)) return;

            if (qty <= 0) _items.Remove(sku);
            else it.Qty = qty;
        }
    }
}
