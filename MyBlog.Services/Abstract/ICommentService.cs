using MyBlog.Shared.Utilities.Results.Abtracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Abstract
{
    public interface ICommentService
    {
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByIsNonDeletedAsync();
    }
}
