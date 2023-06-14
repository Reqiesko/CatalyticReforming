function exampleFunction()
    % Определение границ интервала для поиска минимума и максимума
    xLower = 0.1;
    xUpper = 5;

    % Нахождение минимума функции
    [xMin, fMin] = fminbnd(@(x) -(x + log(x) - x^3), xLower, xUpper);

    % Нахождение максимума функции
    [xMax, fMax] = fminbnd(@(x) (x + log(x) - x^3), xLower, xUpper);

    % Создание массива значений x для построения графика
    x = linspace(xLower, xUpper, 100);
    y = x + log(x) - power(x, 3);

    % Построение графика функции
    plot(x, y);
    hold on;
    scatter(xMin, -fMin, 'r', 'filled');
    scatter(xMax, fMax, 'g', 'filled');
    xlabel('x');
    ylabel('F');
    title('Minimization and Maximization of F = x + log(x) - x^3');
    legend('F = x + log(x) - x^3', 'Min', 'Max');

    % Вывод результатов в виде таблицы
    results = table(['Min'; 'Max'], [xMin; xMax], [-fMin; fMax], 'VariableNames', {'Type', 'x', 'F'});
    disp(results);
end
