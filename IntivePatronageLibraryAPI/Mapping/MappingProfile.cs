using AutoMapper;
using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.DTOs;

namespace IntivePatronageLibraryAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDTO>()
                .ForMember(dest => dest.Books,
                    opt => opt.MapFrom(src => src.Books.Select(BookSelector).ToList()));
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.Authors,
                    opt => opt.MapFrom(src => src.Authors.Select(AuthorSelector).ToList()));
            CreateMap<BookDTO, Book>();
            CreateMap<AuthorDTO, Author>();
        }

        //To remove cycles
        private Book BookSelector(Book x)
        {
            x.Authors.Clear();
            return x;
        }


        //To remove cycles
        private Author AuthorSelector(Author x)
        {
            x.Books.Clear();
            return x;
        }
    }
}