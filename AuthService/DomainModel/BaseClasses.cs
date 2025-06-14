﻿using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthService.DomainModel
{
    public interface IEntity
    {
       
        int Id { get; set; }
    }

    public interface IAuditableEntity : IEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
    public class BaseAuditableEntity : IAuditableEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }      
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }

}