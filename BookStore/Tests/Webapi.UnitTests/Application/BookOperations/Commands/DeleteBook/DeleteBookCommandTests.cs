﻿using AutoMapper;
using BookStoreWebApi.Application.BookOperations.CreateBook;
using BookStoreWebApi.Application.BookOperations.DeleteBook;
using BookStoreWebApi.DBOperations;
using BookStoreWebApi.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSetup;
using Xunit;

namespace Webapi.UnitTests.Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;


        public DeleteBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyNotExistBookTitleGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange(Hazirliq)

            DeleteBookCommand command = new(_context)
            {
                BookId = 10
            };

            //act(Ise Salma) & assert(Tesdileme)
            FluentActions.Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitab tapılmadı");

        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_shouldBeDeleted()
        {
            //arrenge
            Book testBook = new Book() { Id = 10, Title = "WhenValidInputsAreGiven_Book_shouldBeDeleted", PageCount = 100, PublishDate = new DateTime(1990, 1, 22), GenreId = 1 };
            _context.Books.Add(testBook);
            _context.SaveChanges();


            DeleteBookCommand command = new(_context)
            {
                BookId = 10
            };


            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(b => b.Id == command.BookId);
            book.Should().BeNull();

        }




    }
}