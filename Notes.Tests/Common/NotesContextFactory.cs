﻿using Microsoft.EntityFrameworkCore;
using Notes.Persistence;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common
{
    public class NotesContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();

        public static Guid NotesIdForDelete = Guid.NewGuid();
        public static Guid NotesIdForUpdate = Guid.NewGuid();

        public static NotesDbContext Create()
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new NotesDbContext(options);
            context.Database.EnsureCreated();
            context.Notes.AddRange(
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details1",
                    EditDate = null,
                    Id = Guid.Parse("FC0769E2-754A-4262-82FC-ACCF5FB66A24"),
                    Title = "Title1",
                    UserId = UserAId
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details2",
                    EditDate = null,
                    Id = Guid.Parse("7F320EDF-8DD1-4F81-B152-351433C9B39D"),
                    Title = "Title2",
                    UserId = UserBId
                },
                  new Note
                {
                      CreationDate = DateTime.Today,
                      Details = "Details3",
                      EditDate = null,
                      Id = NotesIdForDelete,
                      Title = "Title3",
                      UserId = UserAId
                },
                  new Note
                  {
                      CreationDate = DateTime.Today,
                      Details = "Details4",
                      EditDate = null,
                      Id = NotesIdForUpdate,
                      Title = "Title4",
                      UserId = UserBId
                  });
            context.SaveChanges();
            return context;
        }

        public static void Destroy(NotesDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
