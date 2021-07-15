using AutoMapper;
using MyBlog.Data.Abstract;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.CommentDtos;
using MyBlog.Services.Abstract;
using MyBlog.Services.Utilities;
using MyBlog.Shared.Utilities.Results.Abtracts;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Threading.Tasks;

namespace MyBlog.Services.Concrete
{
    public class CommentManager :ManagerBase, ICommentService
    {

        //Dolu halleri Base Class ımızdan gelecektir
        public CommentManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<IDataResult<CommentDto>> GetAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                return new DataResult<CommentDto>(ResultStatus.Success, new CommentDto
                {
                    Comment = comment,
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, new CommentDto
            {
                Comment = null,
            }, Messages.Comment.NotFound(isPlural: false));
        }

        public async Task<IDataResult<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await UnitOfWork.Comments.AnyAsync(c => c.Id == commentId);
            if (result)
            {
                var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
                var commentUpdateDto = Mapper.Map<CommentUpdateDto>(comment);
                return new DataResult<CommentUpdateDto>(ResultStatus.Success, commentUpdateDto);
            }
            else
            {
                return new DataResult<CommentUpdateDto>(ResultStatus.Error, null, Messages.Comment.NotFound(isPlural: false));
            }
        }

        public async Task<IDataResult<CommentListDto>> GetAllAsync()
        {
            //Article bilgisini Include ettik
            var comments = await UnitOfWork.Comments.GetAllAsync(null,i=>i.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, new CommentListDto
            {
                Comments = null,
            }, Messages.Comment.NotFound(isPlural: true));
        }

        public async Task<IDataResult<CommentListDto>> GetAllByDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => c.IsDeleted, i => i.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error,new CommentListDto
            {
                Comments = null,
            }, Messages.Comment.NotFound(isPlural: true));
        }

        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => !c.IsDeleted, i => i.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, new CommentListDto
            {
                Comments = null,
            }, Messages.Comment.NotFound(isPlural: true));
        }

        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => !c.IsDeleted && c.IsActive, i => i.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, new CommentListDto
            {
                Comments = null,
            }, Messages.Comment.NotFound(isPlural: true));
        }

        public async Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto)
        {
            var article = await UnitOfWork.Articles.GetAsync(a=>a.Id==commentAddDto.ArticleId);

            //Eklenen yoruma ait makale bulunamadıysa
            if (article==null)
            {
                return new DataResult<CommentDto>(ResultStatus.Error, new CommentDto
                {
                    Comment = null,
                }, Messages.Article.NotFound(false));
            }

            var comment = Mapper.Map<Comment>(commentAddDto);
            var addedComment = await UnitOfWork.Comments.AddAsync(comment);

           //Makaleye bir yorum eklendiyse yorum sayısını arttırmamız gerekiyor.
            article.CommentsCount += 1;
            await UnitOfWork.Articles.UpdateAsync(article);
            
            await UnitOfWork.SaveAsync();

            return new DataResult<CommentDto>(ResultStatus.Success,new CommentDto
            {
                Comment = addedComment,
            }, Messages.Comment.Add(commentAddDto.CreatedByName));
        }

        public async Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, string modifiedByName)
        {
            var oldComment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentUpdateDto.Id);
            var comment = Mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);
            comment.ModifiedByName = modifiedByName;
            var updatedComment = await UnitOfWork.Comments.UpdateAsync(comment);
            updatedComment.Article = await UnitOfWork.Articles.GetAsync(i=>i.Id==updatedComment.Id);
            await UnitOfWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success,new CommentDto
            {
                Comment = updatedComment,
            }, Messages.Comment.Update(comment.CreatedByName));
        }

        public async Task<IDataResult<CommentDto>> DeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId,c=>c.Article);
            if (comment != null)
            {
                var article = comment.Article;
                comment.IsDeleted = true;
                comment.IsActive = false;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var deletedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                article.CommentsCount -= 1;
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveAsync();
                return new DataResult<CommentDto>(ResultStatus.Success,  new CommentDto
                {
                    Comment = deletedComment,
                }, Messages.Comment.Delete(deletedComment.CreatedByName));
            }
            return new DataResult<CommentDto>(ResultStatus.Error, new CommentDto
            {
                Comment = null,
            }, Messages.Comment.NotFound(isPlural: false));
        }

        public async Task<IResult> HardDeleteAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId,c=>c.Article);
            if (comment != null)
            {
                //Yorum daha önce geçici olarak silinmiş bir yorum mu diye kontrol ediyoruz.Diğer türlü sayıyı iki kez azaltmış olacağız
                if (comment.IsDeleted)
                {
                    await UnitOfWork.Comments.DeleteAsync(comment);
                    await UnitOfWork.SaveAsync();
                    return new Result(ResultStatus.Success, Messages.Comment.HardDelete(comment.CreatedByName));
                }
                var article = comment.Article;
                await UnitOfWork.Comments.DeleteAsync(comment);
                article.CommentsCount = await UnitOfWork.Comments.CountAsync(c=>c.ArticleId==article.Id && !c.IsDeleted);
                await UnitOfWork.Articles.UpdateAsync(article); //Güncellenen Yorum sayısı verildi
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Comment.HardDelete(comment.CreatedByName));
            }
            return new Result(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync();
            if (commentsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, -1, $"Beklenmeyen bir hata ile karşılaşıldı.");
            }
        }

        public async Task<IDataResult<int>> CountByIsNonDeletedAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync(c => !c.IsDeleted);
            if (commentsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, -1, $"Beklenmeyen bir hata ile karşılaşıldı.");
            }
        }

        public async Task<IDataResult<CommentDto>> ApproveAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(i => i.Id == commentId, a => a.Article);
            if (comment != null)
            {
                var article = comment.Article;
                comment.IsActive = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;

                var updatedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                article.CommentsCount = await UnitOfWork.Comments.CountAsync(c => c.ArticleId == article.Id && !c.IsDeleted);
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveAsync();

                return new DataResult<CommentDto>(ResultStatus.Success, new CommentDto { Comment = updatedComment }, Messages.Comment.Approve(commentId));
            }

            return new DataResult<CommentDto>(ResultStatus.Error, null, Messages.Comment.NotFound(false));
        }

        public async Task<IDataResult<CommentDto>> UndoDeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId, c => c.Article);
            if (comment != null)
            {
                var article = comment.Article;
                comment.IsDeleted = false;
                comment.IsActive = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var undoDeletedComment = await UnitOfWork.Comments.UpdateAsync(comment);

                article.CommentsCount += 1;

                await UnitOfWork.Articles.UpdateAsync(article); //Güncellenen Yorum sayısı verildi

                await UnitOfWork.SaveAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, new CommentDto
                {
                    Comment = undoDeletedComment,
                }, Messages.Comment.NonDelete(undoDeletedComment.CreatedByName));
            }
            return new DataResult<CommentDto>(ResultStatus.Error, new CommentDto
            {
                Comment = null,
            }, Messages.Comment.NotFound(isPlural: false));
        }
    }
}
