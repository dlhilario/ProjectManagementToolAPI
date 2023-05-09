using Moq;
using projectManagementToolWebAPI.Model;
using System;
using System.Collections.Generic;

namespace projectManagementToolWebAPITests
{
    public class TestHelper
    {
        public int userId = 2;
        public int companyId = 3;

        public Projects TestProjects()
        {
            Projects project = new Projects()
            {
                ProjectName = It.IsAny<string>(),
                StartDate = DateTime.Now,
                EstimatedCompletionDate = DateTime.Now,
                Status = StatusEnum._ACTIVE_.ToString(),
                CompanyID = companyId,
                ProjectScope = It.IsAny<string>(),
                CommentList = new List<Comments>()
                {
                   new Comments()
                   {
                       Comment = It.IsAny<string>(),
                       UpdatedByUserID = userId,
                       Time_Stamp = DateTime.Now
                      
                    }
                },
                StreetNumber = It.IsAny<int>(),
                StreetAddress = It.IsAny<string>(),
                City = It.IsAny<string>(),
                State = It.IsAny<string>(),
                ZipCode = It.IsAny<int>(),
                Lot = It.IsAny<string>(),
                Zone = It.IsAny<string>(),
                MaterialList = new List<MaterialList>()
                {
                    new MaterialList()
                    {
                        InvoiceNumber = It.IsAny<string>(),
                        ItemDescription = It.IsAny<string>(),
                        ItemName = It.IsAny<string>(),
                        ItemQuantity = It.IsAny<int>(),
                        Price = It.IsAny<int>(),
                        PurchaseDate = DateTime.Now
                    }
                }


            };

            return project;
        }

    }
}
