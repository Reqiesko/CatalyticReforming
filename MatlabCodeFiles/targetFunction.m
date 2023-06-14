function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()
% Определение параметров функции
a = 15.802;
b = 0.03155;
c = 0.95975;
d = 2.4206;
a1 = 32.181;
b1 = 0.08775;
c1 = 0.5253;
d1 = 3.57;
yn = 29;
ya = 7;
parameter = 'T';
% Создание таблицы
T = 495;
G = 8.87:0.1:16.04;
[TT, GG] = meshgrid(T, G);
F = abs(a - b * TT + c * GG - d*(yn + ya));
tableData = [TT(:), GG(:), F(:)];
tableHeaders = {'T', 'G', 'F'};
tableResult = array2table(tableData, 'VariableNames', tableHeaders);
% Отображение таблицы в новом окне
figure;
uitable('Data', tableData, 'ColumnName', tableHeaders);
% Построение графика в новом окне
figure;
if strcmp(parameter, 'G')
plot(T, F);
title('Зависимость функции при различных Т');
xlabel('T');
ylabel('F');
elseif strcmp(parameter, 'T')
plot(G, F);
title('Зависимость функции при различных G');
xlabel('G');
ylabel('F');
end
% Поиск оптимального значения параметра методом золотого сечения(с помощью fminbnd)
if strcmp(parameter, 'T')
f = @(x)abs(a - b * T + c * x - d * (yn + ya));
[optimalValue, ~] = fminbnd(f, G(1), G(end));
elseif strcmp(parameter, 'G')
f = @(x)abs(a - b * x + c * G - d * (yn + ya));
[optimalValue, ~] = fminbnd(f, T(1), T(end));
end
     % Расчет октанового числа при оптимальном значении параметра
    if strcmp(parameter, 'T')
       optimalOctaneNumber = abs(a1 - b1 * T + c1 * optimalValue - d1 * (yn + ya));
    elseif strcmp(parameter, 'G')
        optimalOctaneNumber = abs(a1 - b1 * optimalValue + c1 * G - d1 * (yn + ya));
    end
    % Проверка критерия октанового числа
    targetOctaneNumber = 60;  % Критериальное значение октанового числа
    if optimalOctaneNumber < targetOctaneNumber
        fprintf('Не удалось получить решение, это может быть вызвано слишком большим значением критериального ограничения, попробуйте его снизить.\n');
    end