using Newtonsoft.Json;

namespace FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedIncome;

public class FixedIncomeResponse
{
    [JsonProperty("gross_profit")]
    public decimal ProfitAfterTaxes { get; set; }
    
    [JsonProperty("net_profit")]
    public decimal ProfitBeforeTaxes { get; set; }
    
    [JsonProperty("invested_amount")]
    public decimal Amount { get; set; }
    
    [JsonProperty("final_net_amount")]
    public decimal FinalNetAmount { get; set; }
    
    [JsonProperty("final_gross_amount")]
    public decimal FinalGrossAmount { get; set; }

    [JsonProperty("start_date")]
    public DateTime StartDate { get; set; }
    
    [JsonProperty("end_date")]
    public DateTime EndDate { get; set; }
    
    [JsonProperty("total_in_days")]
    public int TotalInDays { get; set; }
}