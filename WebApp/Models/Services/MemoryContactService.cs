namespace WebApp.Models.Services
{     
    public class MemoryContactService : IContactService
    { 
        private static Dictionary<int, ContactModel> _contacts = new()
    { 
        {
            1, new ContactModel()
            {
                Category = Category.Friend, Id = 1, First_name = "michal", Last_name = "krul", Email = "jestemkrulem@gmail.com", Phonenumber = "999888777", Date_of_Birth = new DateOnly(2000, 10, 30)
            }
        },
        {
        2, new ContactModel()
        {
            Category = Category.Business, Id = 2, First_name = "michal", Last_name = "krol", Email = "niejestemkrulem@gmail.com", Phonenumber = "111222333", Date_of_Birth = new DateOnly(2002, 10, 30)
        }}
    };
        private static int _currentId = 2;
        public void Add(ContactModel model)
        {
            model.Id = ++_currentId;
            _contacts.Add(model.Id, model);
        }
        public void Update(ContactModel model)
        {
           if(_contacts.ContainsKey(model.Id))
           {
                _contacts[model.Id] = model;
           }
        }   
        public void Delete(int id)
        {
            _contacts.Remove(id);
        }
        public List<ContactModel> GetAll()
        {
            return _contacts.Values.ToList();
        }   
        public ContactModel? GetById(int id)
        {
            if (_contacts.ContainsKey(id))
            {
                return _contacts[id];
            }
            return null;
        }

        public List<OrganizationEntity> GetAllOrganizaions()
        {
            throw new NotImplementedException();
        }
    } 
}     