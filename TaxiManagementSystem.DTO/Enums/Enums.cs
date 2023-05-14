namespace TaxiManagementSystem.DTO.Enums
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum TaxiStatus
    {
        Occupied,
        Unoccupied,
        NotInUse
    }

    public enum TaxiTypes
    {
        Small,
        Combi,
        Van,
        NotInUse
    }

    public enum RideStatus
    {
        Completed,
        Cancelled,
        Accepted,
        Rejected,
        New,
        NotInUse
    }

    public enum Roles
    {
        Admin,
        Driver,
        User
    }
}
