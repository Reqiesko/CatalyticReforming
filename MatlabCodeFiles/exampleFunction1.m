function exampleFunction1()
    % Определение границ интервала для поиска минимума и максимума
    xLower = -10;
    xUpper = 10;

    % Нахождение минимума функции
    [xMin, fMin] = fminbnd(@(x) (x^4 + x^2 + x + 1), xLower, xUpper);

    % Нахождение максимума функции
    [xMax, fMax] = fminbnd(@(x) -(x^4 + x^2 + x + 1), xLower, xUpper);

    % Создание массива значений x для построения графика
    x = linspace(xLower, xUpper, 100);
    y = x.^4 + x.^2 + x + 1;

    % Построение графика функции
    plot(x, y);
    hold on;
    scatter(xMin, fMin, 'r', 'filled');
    scatter(xMax, -fMax, 'g', 'filled');
    xlabel('x');
    ylabel('F');
    title('Minimization and Maximization of F = x^4 + x^2 + x + 1');
    legend('F = x^4 + x^2 + x + 1', 'Min', 'Max');

    % Вывод результатов в виде таблицы
    results = table(['Min'; 'Max'], [xMin; xMax], [fMin; -fMax], 'VariableNames', {'Type', 'x', 'F'});
    disp(results);
end