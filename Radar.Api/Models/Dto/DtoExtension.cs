﻿using Microsoft.EntityFrameworkCore;
using Radar.Api.Data;
using Radar.Api.Models.Dto;

namespace Radar.Api.Models.Dto
{
    public static class DtoExtension
    {
        #region Local
        public static Local ToModel(this LocalCreateDto localDto)
        {
            return new()
            {
                LocalId = localDto.LocalId,
                Nome = localDto.Nome,
                Descricao = localDto.Descricao,
                Endereco = localDto.Endereco,
                Verificado = localDto.Verificado
            };
        }

        public static LocalReadDto ToReadDto(this Local local)
        {
            return new()
            {
                LocalId = local.LocalId,
                Nome = local.Nome,
                Descricao = local.Descricao,
                Endereco = local.Endereco,
                Verificado = local.Verificado
            };
        }

        public static List<LocalReadDto> ToReadDto(this List<Local> locals)
        {
            List<LocalReadDto> localReadDtos = new();

            foreach (Local local in locals)
            {
                LocalReadDto localReadDto = new()
                {
                    LocalId = local.LocalId,
                    Nome = local.Nome,
                    Descricao = local.Descricao,
                    Endereco = local.Endereco,
                    Verificado = local.Verificado
                };

                localReadDtos.Add(localReadDto);
            }

            return localReadDtos;
        }
        #endregion Local

        #region Pessoa
        public static Pessoa ToModel(this PessoaCreateDto pessoaDto)
        {
            return new()
            {
                PessoaId = pessoaDto.PessoaId,
                Nome = pessoaDto.Nome,
                Email = pessoaDto.Email,
                Login = pessoaDto.Login,
                Senha = pessoaDto.Senha,
                Descricao = pessoaDto.Descricao,
                DataNascimento = pessoaDto.DataNascimento
            };
        }

        public static PessoaReadDto ToReadDto(this Pessoa pessoa)
        {
            return new()
            {
                PessoaId = pessoa.PessoaId,
                Nome = pessoa.Nome,
                Email = pessoa.Email,
                Login = pessoa.Login,
                Senha = pessoa.Senha,
                Descricao = pessoa.Descricao,
                DataNascimento = pessoa.DataNascimento
            };
        }

        public static List<PessoaReadDto> ToReadDto(this List<Pessoa> pessoas)
        {
            List<PessoaReadDto> pessoaReadDtos = new();

            foreach (Pessoa pessoa in pessoas)
            {
                PessoaReadDto PessoaReadDto = new()
                {
                    PessoaId = pessoa.PessoaId,
                    Nome = pessoa.Nome,
                    Email = pessoa.Email,
                    Login = pessoa.Login,
                    Senha = pessoa.Senha,
                    Descricao = pessoa.Descricao,
                    DataNascimento = pessoa.DataNascimento
                };

                pessoaReadDtos.Add(PessoaReadDto);
            }

            return pessoaReadDtos;
        }
        #endregion Pessoa

        #region Post
        public static Post ToModel(this PostCreateDto postDto, RadarContext context)
        {
            return new()
            {
                PostId = postDto.PostId,
                PessoaId = postDto.PessoaId,
                LocalId = postDto.LocalId,
                Conteudo = postDto.Conteudo,
                Avaliacao = postDto.Avaliacao,
                DataPostagem = postDto.DataPostagem,
                Likes = 0,
                Local = context.Locals.Single(local => local.LocalId == postDto.LocalId),
                Pessoa = context.Pessoas.Single(pessoa => pessoa.PessoaId == postDto.PessoaId)
            };
        }
        public static List<PostReadDto> ToReadDto(this List<Post> posts, RadarContext context)
        {
            List<PostReadDto> postReadDtos = new();

            foreach (Post post in posts)
            {
                PostReadDto postReadDto = new()
                {
                    PostId = post.PostId,
                    Pessoa = context.Pessoas.Single(pessoa => pessoa.PessoaId == post.PessoaId),
                    Local = context.Locals.Single(local => local.LocalId == post.PessoaId),
                    Conteudo = post.Conteudo,
                    Avaliacao = post.Avaliacao,
                    DataPostagem = post.DataPostagem,
                    Curtidas = context.Curtida.Count(curtida => curtida.PostIdCurtido == post.PostId)
                };

                postReadDtos.Add(postReadDto);
            }

            return postReadDtos;
        }

        public static PostReadDto ToReadDto(this Post post, RadarContext context)
        {
            return new()
            {
                PostId = post.PostId,
                Pessoa = context.Pessoas.Single(pessoa => pessoa.PessoaId == post.PessoaId),
                Local = context.Locals.Single(local => local.LocalId == post.PessoaId),
                Conteudo = post.Conteudo,
                Avaliacao = post.Avaliacao,
                DataPostagem = post.DataPostagem,
                Curtidas = context.Curtida.Count(curtida => curtida.PostIdCurtido == post.PostId)
            };
        }
        #endregion Post
    }
}
