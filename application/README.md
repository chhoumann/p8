
# Setup instructions
How to set up frontend:
1. Install PNPM (npm install -g pnpm)
2. cd into application dir
3. pnpm install
4. pnpm prisma db push

# Cypress
## Commands
Open cypress using ```npx cypress open```. This will pop up with a UI. Here, you can see your tests and see the code execute.

## Writing tests/specifications
In ./cypress/e2e is a simple example of a todo component. To create a test, create cypress file (xxx.cy.ts) in that directory.
When creating tests, a single file should fully specify the behavior of the component under test.
That is, a single file should test the full functionality of the delivery/usecase.
