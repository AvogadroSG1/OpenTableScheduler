using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Peter.OpenTable.Models
{
    public class OpenTableReservationRequest
    {
        //{\"covers\":{0},\"dateTime\":\"{1}\",\"isRedesign\":true,\"availabilityToken\":null,\"correlationId\":\"null\"}
        public int covers { get; set; }
        public string dateTime { get; set; }

        [JsonIgnore]
        public int restuarantID { get; set; }
        public bool isRedesign { get; set; } = true;
        public string availabilityToken { get; set; } = null;
        public string correlationId { get; set; } = null;
    }

    public class SameDayAvailability
    {
        [JsonPropertyName("times")]
        public List<Time> Times { get; set; }

        [JsonPropertyName("noTimesMessage")]
        public string NoTimesMessage { get; set; }
    }

    public class Time
    {
        [JsonPropertyName("covers")]
        public int Covers { get; set; }

        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }

        [JsonPropertyName("timeString")]
        public string TimeString { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }

        [JsonPropertyName("offers")]
        public List<object> Offers { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        [JsonPropertyName("premiumAccessPriceAmount")]
        public object PremiumAccessPriceAmount { get; set; }

        [JsonPropertyName("ticketed")]
        public bool Ticketed { get; set; }

        [JsonPropertyName("attributes")]
        public List<string> Attributes { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Timeslot
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("times")]
        public List<Time> Times { get; set; }

        [JsonPropertyName("noTimesMessage")]
        public object NoTimesMessage { get; set; }
    }

    public class MultiDaysAvailability
    {
        [JsonPropertyName("nextAvailabilityIndex")]
        public int NextAvailabilityIndex { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("timeslots")]
        public List<Timeslot> Timeslots { get; set; }
    }

    public class DeliveryPartner
    {
        [JsonPropertyName("deliveryPartnerName")]
        public string DeliveryPartnerName { get; set; }

        [JsonPropertyName("menuUrl")]
        public string MenuUrl { get; set; }
    }

    public class Restaurant
    {
        [JsonPropertyName("rid")]
        public int Rid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }

        [JsonPropertyName("showReviews")]
        public bool ShowReviews { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("reviewsCount")]
        public int ReviewsCount { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonPropertyName("primaryCuisine")]
        public string PrimaryCuisine { get; set; }

        [JsonPropertyName("restaurantProfileUrl")]
        public string RestaurantProfileUrl { get; set; }

        [JsonPropertyName("recentReservationCount")]
        public int RecentReservationCount { get; set; }

        [JsonPropertyName("showDeliveryCarousel")]
        public bool ShowDeliveryCarousel { get; set; }

        [JsonPropertyName("deliveryPartners")]
        public List<DeliveryPartner> DeliveryPartners { get; set; }
    }

    public class NoAvailabilityRestaurants
    {
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; }

        [JsonPropertyName("correlationId")]
        public string CorrelationId { get; set; }

        [JsonPropertyName("restaurants")]
        public List<Restaurant> Restaurants { get; set; }
    }

    public class NoTimesReason
    {
        [JsonPropertyName("Reason")]
        public string Reason { get; set; }

        [JsonPropertyName("minPartySize")]
        public int MinPartySize { get; set; }

        [JsonPropertyName("MaxPartySize")]
        public int MaxPartySize { get; set; }

        [JsonPropertyName("MaxDaysInAdvance")]
        public int MaxDaysInAdvance { get; set; }

        [JsonPropertyName("SameDayCutoff")]
        public object SameDayCutoff { get; set; }

        [JsonPropertyName("EarlyCutoff")]
        public object EarlyCutoff { get; set; }

        [JsonPropertyName("AllowNextAvailable")]
        public bool AllowNextAvailable { get; set; }
    }

    public class OpenTableAPIResponseBase
    {
        [JsonPropertyName("sameDayAvailability")]
        public SameDayAvailability SameDayAvailability { get; set; }

        [JsonPropertyName("multiDaysAvailability")]
        public MultiDaysAvailability MultiDaysAvailability { get; set; }

        [JsonPropertyName("noAvailabilityRestaurants")]
        public NoAvailabilityRestaurants NoAvailabilityRestaurants { get; set; }

        [JsonPropertyName("noTimesReason")]
        public NoTimesReason NoTimesReason { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Variables
    {
        [JsonPropertyName("term")]
        public string Term { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    public class PersistedQuery
    {
        [JsonPropertyName("version")]
        public int Version { get; set; } = 1;

        [JsonPropertyName("sha256Hash")]
        public string Sha256Hash { get; set; } = null;
    }

    public class Extensions
    {
        [JsonPropertyName("persistedQuery")]
        public PersistedQuery PersistedQuery { get; set; }
    }

    public class OpenTableAutoCompleteQuery
    {
        [JsonPropertyName("operationName")]
        public string OperationName { get; set; } = "Autocomplete";

        [JsonPropertyName("variables")]
        public Variables Variables { get; set; }

        [JsonPropertyName("extensions")]
        public Extensions Extensions { get; set; }
    }

    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class AddressResponse
    {
        [JsonPropertyName("address1")]
        public string Address1 { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("rid")]
        public int Rid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("addressResponse")]
        public AddressResponse AddressResponse { get; set; }

        [JsonPropertyName("productType")]
        public List<string> ProductType { get; set; }

        [JsonPropertyName("testRestaurant")]
        public bool TestRestaurant { get; set; }
    }

    public class OpenTableRestaurantQueryResponse
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
    }

    public class OpenTableRestaurantQueryRequest
    {
        public string query { get; set; }
        public int pageSize { get; set; } = 100;

        public override string ToString() => $"https://www.opentable.com/widget/reservation/restaurant-search?query={query}&pageSize={pageSize}";
    }


}