# 📘 Практика по C#: Делегаты, Action/Func, Лямбды, События

Набор практических заданий для отработки тем: делегаты, групповой вызов, ковариантность/контрвариантность (вводно), `Action<T>` и `Func<T>`, лямбда-выражения, события и аксессоры событий.

## 🎯 Ключевые темы
- **Делегаты**: собственные типы делегатов, передача поведения, базовые проверки.
- **Групповой вызов** и **адресация методов** у делегатов (мультикаст).
- **Ковариантность/контрвариантность** (обсуждается на лекции, применимо к `Action/Func`).
- **Action<T> и Func<T>**: конвейер действий, композиция функций, фильтрация и проекция.
- **Лямбда-выражения**: краткая запись для делегатов.
- **События и аксессоры событий**: управление подписками, ограничения, уведомление о состояниях.

---

## 📝 Как работать с задачами
1. Откройте решение `App.sln`.
2. Перейдите в проект `App` и директории задач. Внутри каждой задачи есть файл с ТОЛЬКО объявлением `namespace` — реализуйте весь требуемый код самостоятельно.
3. Запустите проект тестов `App.Test` (NUnit) — изначально тесты могут не компилироваться/падать. Ваша цель — реализовать решения так, чтобы все тесты стали зелёными.
4. Команда для запуска: `dotnet test`.

---

## 📁 Структура проекта
```
/App
  /Topics
    /Delegates
      /T1_BasicDelegate
        Stub.cs              # только namespace
    /ActionDelegates
      /T2_ActionPipeline
        Stub.cs              # только namespace
    /FuncDelegates
      /T3_FuncFilterMap
        Stub.cs              # только namespace
    /Events
      /T4_CustomEvent
        Stub.cs              # только namespace
/App.Test
  # Зеркальная структура с тестами на каждую задачу
/.github/workflows/dotnet.yml  # CI: сборка + тесты на каждый push/PR
```

---

## ✅ Задачи

### T1_BasicDelegate — собственные делегаты и работа с массивами
- Реализуйте в неймспейсе `App.Topics.Delegates.T1_BasicDelegate`:
  - Типы делегатов:
    - `public delegate int IntUnary(int x);`
    - `public delegate bool IntPredicate(int x);`
  - Класс `IntAlgorithms` со статическими методами:
    - `int[] Map(int[] source, IntUnary f)` — применяет делегат к каждому элементу и возвращает новый массив.
    - `int[] Filter(int[] source, IntPredicate predicate)` — возвращает новый массив только из элементов, удовлетворяющих предикату.
- Требования и крайние случаи:
  - `source == null` → `ArgumentNullException`.
  - Делегат/предикат `null` → `ArgumentNullException`.
  - Пустой массив → пустой результат.
  - Не изменяйте исходный массив.

### T2_ActionPipeline — мультикаст-вызов на основе Action
- Неймспейс: `App.Topics.ActionDelegates.T2_ActionPipeline`.
- Реализуйте класс `ActionPipeline`:
  - `public static int InvokeAll(string input, params Action<string>[] handlers)`
    - Вызывает обработчики по порядку.
    - Возвращает число УСПЕШНО вызванных обработчиков.
    - Если один из обработчиков бросает исключение — дальнейшие не вызываются, исключение пробрасывается наружу.
    - `handlers == null` → `ArgumentNullException`.
    - `null` внутри массива обработчиков пропускается (не вызывает ошибок и не учитывается в счётчике).
- Проверьте также сценарий с единым мультикаст-делегатом, собранным через `Delegate.Combine`.

### T3_FuncFilterMap — Func и лямбды, мини-LINQ
- Неймспейс: `App.Topics.FuncDelegates.T3_FuncFilterMap`.
- Реализуйте класс `LinqLite`:
  - `public static IEnumerable<TResult> FilterMap<T, TResult>(IEnumerable<T> source, Func<T, bool> predicate, Func<T, TResult> selector)`
    - Отфильтровать `source` по `predicate` и преобразовать к `TResult` через `selector`.
- Требования и крайние случаи:
  - Любой аргумент `null` → `ArgumentNullException`.
  - Пустая последовательность → пустой результат.
  - Разрешается как немедленная реализация (список), так и ленивый итератор (через `yield`).

### T4_CustomEvent — событие с аксессорами и ограничением подписчиков
- Неймспейс: `App.Topics.Events.T4_CustomEvent`.
- Реализуйте:
  - `public sealed class ThresholdReachedEventArgs : EventArgs` с `public int Value { get; }`, `public int Threshold { get; }`.
  - `public sealed class Counter`:
    - Конструктор: `Counter(int threshold, int maxSubscribers = 3)`; `threshold > 0`, иначе `ArgumentOutOfRangeException`.
    - Свойство `public int Value { get; private set; }`.
    - Метод `public void Increment(int delta = 1)`; `delta > 0`, иначе `ArgumentOutOfRangeException`.
    - Событие `public event EventHandler<ThresholdReachedEventArgs> ThresholdReached` с пользовательскими аксессорами `add/remove`:
      - `add`: `null` → `ArgumentNullException`.
      - Не допускать дубликатов подписчиков (повторное добавление того же делегата игнорируется).
      - Ограничить количество подписчиков `<= maxSubscribers`; при превышении → `InvalidOperationException`.
      - `remove`: удаление отсутствующего обработчика — тихо игнорируется; `null` — игнорируется.
    - Срабатывание события: когда `Value` после инкремента становится кратным `threshold` (например, при `threshold=5` — на 5, 10, 15, ...). В `EventArgs` передавайте текущие `Value` и `Threshold`.

---

## 🧪 Запуск тестов
- Через IDE (Test Explorer) или командой: `dotnet test`.
- Изначально проект может не компилироваться/тесты будут красными — это нормально. Реализуйте задачи по очереди и добивайтесь «зелёных» тестов.

---

## 💡 Подсказки
- Используйте лямбда-выражения для компактного определения делегатов в тестах и при отладке.
- Не забывайте про проверки аргументов на `null` и эффективную работу с коллекциями.
- Для событий используйте пользовательские аксессоры `add/remove` для контроля подписок.
