function exampleFunction3()
    % Определение границ интервала для поиска минимума и максимума
    xLower = 0.1;
    xUpper = 10;

    % Нахождение минимума функции
    [xMin, fMin] = fminbnd(@(x) (1/x + sqrt(x)), xLower, xUpper);

    % Нахождение максимума функции
    [xMax, fMax] = fminbnd(@(x) -(1/x + sqrt(x)), xLower, xUpper);

    % Создание массива значений x для построения графика
    x = linspace(xLower, xUpper, 100);
    y = 1./x + sqrt(x);

    % Построение графика функции
    plot(x, y);
    hold on;
    scatter(xMin, fMin, 'r', 'filled');
    scatter(xMax, -fMax, 'g', 'filled');
    xlabel('x');
    ylabel('F');
    title('Minimization and Maximization of F = 1/x + sqrt(x)');
    legend('F = 1/x + sqrt(x)', 'Min', 'Max');

    % Вывод результатов в виде таблицы
    results = table(['Min'; 'Max'], [xMin; xMax], [fMin; -fMax], 'VariableNames', {'Type', 'x', 'F'});
    disp(results);
end