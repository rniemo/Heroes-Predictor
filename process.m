fprintf('loading data\n');
data = load('data/game0.txt');
for i = 1:365
    new_data = load(strcat('data/game', num2str(i), '.txt'));
    data = [data; new_data];
end
fprintf('splitting data\n');
% data size is 98,866
trainx = data(1:50000, 3:end);
trainy = data(1:50000, 1);
testx = data(50001:end, 2:end);
testy = data(50001:end, 1);
fprintf('building model\n');
model = fitcsvm(trainx, trainy, 'KernelFunction', 'gaussian');
fprintf('testing model\n');
outputy = predict(model, testx(:, 2:end));
accuracy = sum(outputy == testy) / length(testy)