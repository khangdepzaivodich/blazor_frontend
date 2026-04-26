namespace blazor_frontend.Models;

public class VietnamProvince
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Unit { get; set; }
}

public class VietnamDistrict
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Unit { get; set; }
    public string? province_code { get; set; }
    public string? province_name { get; set; }
    public string? full_name { get; set; }
}

public class VietnamWard
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Unit { get; set; }
    public string? district_code { get; set; }
    public string? district_name { get; set; }
    public string? province_code { get; set; }
    public string? province_name { get; set; }
    public string? full_name { get; set; }
}
