using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Server.Service.src.DTO;

namespace Server.Test.src.Service.Data;

public class ReviewServiceTestData : TheoryData<UpdateReviewsDTO>
{
    public ReviewServiceTestData()
    {
        Add(new UpdateReviewsDTO(3.0, "Okayish product"));
    }
}
