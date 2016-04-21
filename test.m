
accuracies = zeros(2);
for i = 0:35
   tlow = i * 60 + 10; 
   thigh = (i+1) * 60;
   testdatax = testx(testx(:,1)==tlow, :);
   testdatay = testy(testx(:,1)==tlow, :);
   for t = tlow+10:10:thigh
        testdatax = [testdatax; testx(testx(:,1)==t, :)];
        testdatay = [testdatay; testy(testx(:,1)==t, :)];
   end
   outputy = predict(model, testdatax(:, 2:end));
   accuracies(i+1, 1) = sum(outputy == testdatay) / length(testdatay);
   accuracies(i+1, 2) = length(testdatay);
end

%botaxis = gca;

plot(0:35, accuracies(:,1));
title('Kernel SVM Accuracy');
xlabel('Time in game (m)');
ylabel('Accuracy (%)');
yyaxis right;
plot(0:35, accuracies(:,2));
ylabel('# Test games');
ylim([0, 300+max(accuracies(:,2))]);