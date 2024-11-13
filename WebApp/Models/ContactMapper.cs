namespace WebApp.Models;

public class ContactMapper
{
    public static ContactEntity ToEntity(ContactModel arg)
    {
        return new ContactEntity()
        {
            Id = arg.Id, First_name = arg.First_name, Last_name = arg.Last_name, Email = arg.Email,
            Phonenumber = arg.Phonenumber, Date_of_Birth = arg.Date_of_Birth, Category = arg.Category, Organization =arg.Organization , OrganizationId = arg.OrganizationId
        };
    }

    public static ContactModel FromEntity(ContactEntity arg)
    {
        return new ContactModel()
        {
            Id = arg.Id, First_name = arg.First_name, Last_name = arg.Last_name, Email = arg.Email,
            Phonenumber = arg.Phonenumber, Date_of_Birth = arg.Date_of_Birth, Category = arg.Category,Organization =arg.Organization , OrganizationId = arg.OrganizationId
        };
    }
}