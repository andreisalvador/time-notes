using FluentValidation;
using System;

namespace TimeNotes.Core
{
    public abstract class Entity<TEntity> where TEntity : class
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public abstract void Validate();

        protected void Validate(TEntity entity, AbstractValidator<TEntity> validator) =>
            validator.ValidateAndThrow(entity);
    }
}
