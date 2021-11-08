
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace textadventure_backend.Models
{
    public class GumroadSaleRequest
    {
        //public string Currency { get; set; }
        //public string Email { get; set; }
        //public string Ip_Country { get; set; }
        //public string License_Key { get; set; }
        //public int Order_Number { get; set; }
        //public string Permalink { get; set; }
        //public float Price { get; set; }
        //public string Product_Id { get; set; }
        //public string Product_Name { get; set; }
        //public string Purchaser_Id { get; set; }
        //public string Quantity { get; set; }
        //public string Recurrence { get; set; }
        //public string Sale_Id { get; set; }
        //public string Sale_Timestamp { get; set; }
        //public string Subscription_Id { get; set; }
        //public ProductVariants Variants { get; set; }
        //public string Resource_Name { get; set; }

        public string seller_id { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string permalink { get; set; }
        public string product_permalink { get; set; }
        public string short_product_id { get; set; }
        public string email { get; set; }
        public string price { get; set; }
        public string gumroad_fee_charged { get; set; }
        public string can_contant { get; set; }
        public string referrer { get; set; }
        public string sale_id { get; set; }
        public string sale_timestamp { get; set; }
        public string purchaser_id { get; set; }
        public string subscription_id { get; set; }
        public string variants { get; set; }
        public string license_key { get; set; }
        public string ip_country { get; set; }
        public string recurrence { get; set; }
        public string is_gift_receiver_purchase { get; set; }
        public string refunded { get; set; }
        public string resource_name { get; set; }
        public string disputed { get; set; }
        public string disputed_won { get; set; }
    }
}
