namespace Domain.Models;

/*
 * 1) Copy the class file
 * 2) Rename the copied file and ensure the change is reflected in the Class name
 * 3) Add the fields 
 */

public class ModelTemplate
{
    public int Id { get; set; }
    // other fields go here
    public DateTime? CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}

public class TestModel
{
    public string? Test { get; set; } 
}
