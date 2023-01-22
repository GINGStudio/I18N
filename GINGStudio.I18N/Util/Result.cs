using System;

namespace GINGStudio.I18N.Util
{
    public sealed class Result<T>
    {
        private readonly T _value;
        private readonly string? _msg;
        public readonly bool Ok;
        private readonly Exception? _exp;
        public Result(T value)
        {
            _value = value;
            Ok = true;
        }
        
        public Result(Exception exp)
        {
            _exp = exp;
            Ok = false;
        }
        public Result(string message)
        {
            _msg = message;
            Ok = false;
        }
        
        public T Unwrap()
        {
            if (!Ok) throw new Exception(_msg);
            return _value;
        }
        
        public string Message() => Ok ? "Success" : _msg ?? _exp?.Message ?? "No Message";
        
        public static Result<T> NewError(Exception exp) => new Result<T>(exp);
        public static Result<T> NewError(string msg) => new Result<T>(msg);
        public static Result<T> NewValue(T value) => new Result<T>(value);
        
        public static explicit operator T(Result<T> error) => error.Unwrap();
        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static explicit operator Result<T>(string message) => new Result<T>(message);
        public static explicit operator Result<T>(Exception exp) => new Result<T>(exp);
    }
}