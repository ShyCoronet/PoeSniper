using System;

namespace Utils {
    public class Maybe<T> {
        internal Maybe() { }

        public static Maybe<T> TryCreateSuccess(T value) {
            if (value != null)
                return new Success<T>(value);

            return new Failure<T>(new NullReferenceException());
        }

        public static Failure<T> CreateFailure(Exception ex) {
            return new Failure<T>(ex);
        }
    }


    public class Success<T> : Maybe<T> {
        public T Value { get; }

        public Success(T value) {
            Value = value;
        }
    }

    public class Failure<T> : Maybe<T> {
        public Exception Exception { get; }
        public Failure() { }

        public Failure(Exception ex) {
            Exception = ex;
        }
    }
}
