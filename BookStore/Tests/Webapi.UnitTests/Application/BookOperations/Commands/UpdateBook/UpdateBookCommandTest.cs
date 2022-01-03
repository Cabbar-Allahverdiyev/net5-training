﻿using AutoMapper;
using BookStoreWebApi.Application.BookOperations.UpdateBook;
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

namespace Webapi.UnitTests.Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
       
        public UpdateBookCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
           
        }

        [Fact]
        public void WhenAlreadyNotExistBookTitleGiven_InvalidOperationException_ShlouldBeReturn()
        {
            //arrange 
            Book book = new Book() { Id = 11, Title = "WhenAlreadyNotExistBookTitleGiven_InvalidOperationException_ShlouldBeReturn", PageCount = 100, PublishDate = new DateTime(1990, 1, 22), GenreId = 1 };
            _context.Books.Add(book);
            _context.SaveChanges();

            UpdateBookCommand command = new(_context);
            command.BookId =14 ;
            //act & assert 
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitab tapılmadı");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_shouldBeUpdate()
        {
            //arrange
            Book testBook = new Book() { Id = 12, Title = "WhenValidInputsAreGiven_Book_shouldBeUpdate", PageCount = 100, PublishDate = new DateTime(1990, 1, 22), GenreId = 1 };
            _context.Books.Add(testBook);
            _context.SaveChanges();

            UpdateBookCommand command = new(_context);

            UpdateBookModel model = new()
            {
                
                Title = "Lord Of The Rings",
                PageCount = 1000,
                PublishDate = new DateTime(1990, 01, 20),
                GenreId = 1
            };
            command.BookId = 12;
            command.Model = model;

            //act
            FluentActions.Invoking(()=>command.Handle()).Invoke();

            //assert
            var book = _context.Books.SingleOrDefault(b => b.Id == command.BookId);
            book.Should().NotBeNull();
            book.PageCount.Should().Be(model.PageCount);
            book.GenreId.Should().Be(model.GenreId);
            book.PublishDate.Should().Be(model.PublishDate);
        }

    }
}