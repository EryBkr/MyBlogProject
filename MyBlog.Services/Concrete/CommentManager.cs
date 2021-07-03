using AutoMapper;
using MyBlog.Data.Abstract;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.Abtracts;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Concrete
{
    public class CommentManager : ICommentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync();
            if (commentsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, 0, "Beklenmeyen bir hata ile karşılaşıldı");
            }
        }

        public async Task<IDataResult<int>> CountByIsNonDeletedAsync()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync(i=>!i.IsDeleted);
            if (commentsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, 0, "Beklenmeyen bir hata ile karşılaşıldı");
            }
        }
    }
}
