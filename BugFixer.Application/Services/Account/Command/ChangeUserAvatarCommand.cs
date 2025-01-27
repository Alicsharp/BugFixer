using BugFixer.Application.Extensions;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using BugFixer.Application.Statics;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Command
{

    public record ChangeUserAvatarCommand(long UserId, IFormFile userAvatar) : IRequest<OperationResult>;
    public class ChangeUserAvatarCommandHandler : IRequestHandler<ChangeUserAvatarCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserAvatarCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(ChangeUserAvatarCommand request, CancellationToken cancellationToken)
        {
            // دریافت فرمت فایل جدید
            var fileName = Guid.NewGuid() + Path.GetExtension(request.userAvatar.FileName);
 
            var validFormats = new List<string>()
               {
            ".png",
            ".jpg",
             ".jpeg"
               };


            // آپلود فایل جدید
            var uploadResult =  request.userAvatar.UploadFile(fileName, PathTools.UserAvatarServerPath, validFormats);

            if (!uploadResult)
            {
                return OperationResult.Error("فرمت فایل معتبر نیست.");
            }

            // دریافت کاربر از دیتابیس
            var user = await _userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                return OperationResult.Error("کاربر یافت نشد.");
            }

            // حذف عکس قبلی
            if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar != PathTools.DefaultUserAvatar)
            {
                user.Avatar.DeleteFile(PathTools.UserAvatarServerPath);
            }

            // به‌روزرسانی نام فایل در پروفایل کاربر
            user.ChangeAvatar(fileName);

            // ذخیره تغییرات در دیتابیس
            await _userRepository.Save();

            return OperationResult.Success();
        }
    }
}

